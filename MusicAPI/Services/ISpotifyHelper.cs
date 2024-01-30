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
    }
}
