using Microsoft.Extensions.Configuration;
using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MusicAPI.Services
{
    public interface ISpotifyHelper
    {
        Task SaveArtistGenreAndTrackFromSpotifyToDb(string searchQuery);
        Task GetTop100MostPopularArtists();

        Task GetTopTracksByArtist(ArtistDto artist, GenreDto genre, string token);
    }

    public class SpotifyHelper : ISpotifyHelper
    {
        private readonly HttpClient _httpClient;
        private readonly ISpotifyAccountHelper _spotifyAccountHelper;
        private readonly IConfiguration _configuration;
        private readonly IArtistRepository _artistRepository;

        public SpotifyHelper(HttpClient httpClient, ISpotifyAccountHelper spotifyAccountHelper, IConfiguration configuration, IArtistRepository artistRepository)
        {
            _httpClient = httpClient;
            _spotifyAccountHelper = spotifyAccountHelper;
            _configuration = configuration;
            _artistRepository = artistRepository;
        }

        public async Task SaveArtistGenreAndTrackFromSpotifyToDb(string searchArtist)
        {
            if (string.IsNullOrWhiteSpace(searchArtist))
            {
                throw new ArgumentNullException(nameof(searchArtist));
            }

            var token = await _spotifyAccountHelper.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(searchArtist)}&type=track,artist&limit=50");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<SpotifySearchResultDto>(responseStream);

            if (responseObject.tracks != null && responseObject.artists != null)
            {
                List<SongDto> trackList = new List<SongDto>();

                foreach (var spotifyTrack in responseObject.tracks.items)
                {
                    if(spotifyTrack == null)
                    {
                        continue;
                    }

                    SongDto track = new SongDto()
                    {
                        Name = spotifyTrack.name
                    };

                    trackList.Add(track);
                }

                if(responseObject.artists.items.Length > 0)
                {
                    var spotifyArtist = responseObject.artists.items[0];

                    ArtistDto artist = new ArtistDto()
                    {
                        Name = spotifyArtist.name
                    };

                    if(spotifyArtist.genres.Length > 0)
                    {
                        GenreDto genre = new GenreDto()
                        {
                            Title = spotifyArtist.genres[0]
                        };

                        await _artistRepository.AddArtistsGenresAndTracksFromSpotify(artist, genre, trackList);
                    }
                    else
                    {
                        throw new SpotifyGenreNotFoundException();
                    }
                }
                else
                {
                    throw new SpotifyArtistNotFoundException();
                }
            }
            else
            {
                throw new SpotifyArtistNotFoundException();
            }
        }

        public async Task GetTop100MostPopularArtists()
        {
            var token = await _spotifyAccountHelper.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);

            List<ArtistDto> top100Artists = new List<ArtistDto>();
            List<GenreDto> top100ArtistGenres = new List<GenreDto>();

            int offset = 0;

            for (int i = 1; i <= 2; i++)
            {
                using (var httpClient = new HttpClient())
                {

                    var request = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri($"https://api.spotify.com/v1/playlists/4i96DEnCkGkhBRcI9SYuc4/tracks?offset={offset}&limit=50") };
                    request.Headers.Add("Authorization", $"Bearer {token}");

                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();



                    string responseData = await response.Content.ReadAsStringAsync()!;
                    PlaylistResponse playlist = JsonSerializer.Deserialize<PlaylistResponse>(responseData)!;

                    foreach (Item item in playlist.Items)
                    {
                        foreach (Artist1 artist in item.track.artists)
                        {
                            if (!top100Artists.Any(a => a.Name == artist.name))
                            {
                                top100Artists.Add(new ArtistDto
                                {
                                    Name = artist.name,
                                    Description = "",
                                    SpotifyId = artist.id,
                                });

                                top100ArtistGenres.Add(new GenreDto
                                {
                                    Title = artist.genres[0].ToString()
                                }); ;
                            }
                        }
                    }
                }
                offset = 50;
            }

            for (int i = 0; i < top100Artists.Count; i++)
            {
                //await Console.Out.WriteLineAsync($"Artist : {top100Artists[i].Name} , Genre : {top100ArtistGenres[i].Title}");

                //await GetTopTracksByArtist(top100Artists[i] ,token);
            }
        }


        public async Task GetTopTracksByArtist(ArtistDto artist, GenreDto genre, string token)
        {
            using (var httpClient = new HttpClient())
            {
                //dont need to save listatm
                List<SongDto> top10TracksByArtist = new List<SongDto>();

                Console.WriteLine($"sending request for spotifyID : {artist.SpotifyId}");

                var request = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri($"https://api.spotify.com/v1/artists/{artist.SpotifyId}/top-tracks?market=SE") };
                request.Headers.Add("Authorization", $"Bearer {token}");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync()!;
                TopTracksReponse songs = JsonSerializer.Deserialize<TopTracksReponse>(responseData)!;

                foreach (Track track in songs.Tracks)
                {
                    top10TracksByArtist.Add(new SongDto()
                    {
                        Name = track.name
                    }) ;
                    await Console.Out.WriteLineAsync($"Artist : {artist.Name}, Track : {track.name}, Genre :  {genre.Title}");
                }

                await Task.Delay(2000);
                await _artistRepository.AddArtistsGenresAndTracksFromSpotify(artist, genre, top10TracksByArtist);
            }
        }
        
    }
}
