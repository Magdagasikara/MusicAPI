using System.Net.Http.Headers;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Models.ViewModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using MusicAPI.Repositories;
using System.Net;

namespace MusicAPI.Services
{
    public interface ISpotifyHelper
    {
        Task<string> GetToken(string clientId, string clientSecret);
        Task<string> TryGetSthFromSpotify(string token);
        Task<List<ArtistDto>> GetTopAllTime100(int limit, int offset, string token);
        Task<List<SongDto>> GetTopTracksFromArtist(ArtistDto artist, string token);
    }

    public class SpotifyHelper : ISpotifyHelper
    {
        private readonly HttpClient _httpClient;

        public SpotifyHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetToken(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials" }
            });

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(responseStream);

            return authResult.access_token;
        }
        public async Task<string> TryGetSthFromSpotify(string token)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/artists/4Z8W4fKeB5YxbusRsdQVPb");
            request.Headers.Authorization = new AuthenticationHeaderValue(
                           "Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<ArtistDto>> GetTopAllTime100(int limit, int offset,  string token)
        {
            using (var httpClient = new HttpClient())
            {
                List<ArtistDto> top100Artists = new List<ArtistDto>();

                Console.WriteLine("sending request");
                var request = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri("https://api.spotify.com/v1/playlists/5ABHKGoOzxkaa28ttQV9sE/tracks?offset=0&limit=50") };
                request.Headers.Add("Authorization", $"Bearer {token}");
                
                var response = await httpClient.SendAsync(request);


                //Använd ensuresuccessstatuscode istället.
                if (response.IsSuccessStatusCode)
                {
                    {
                        await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
                    }
                    

                    string responseData = await response.Content.ReadAsStringAsync()!;
                    PlaylistResponse playlist = JsonSerializer.Deserialize<PlaylistResponse>(responseData)!;

                    foreach (var item in playlist.Items)
                    {
                        foreach (var artist in item.track.artists)
                        {
                            Console.WriteLine($"{artist.name}");

                            if (!top100Artists.Any(a => a.SpotifyId == artist.id))
                            {
                                top100Artists.Add(new ArtistDto
                                {
                                    Name = artist.name,
                                    //cant get artist description from this call.
                                    Description = "",
                                    SpotifyId = artist.id,
                                });
                            }
                        }
                    }

                    return top100Artists;
                }

                else
                {
                    return top100Artists;
                }
             }
        }

        public async Task<List<SongDto>> GetTopTracksFromArtist(ArtistDto artist, string token)
        {
            using (var httpClient = new HttpClient())
            {
                List<SongDto> topSongs = new List<SongDto>();
                Console.WriteLine($"sending request for spotifyID : {artist.SpotifyId}");
                Console.WriteLine($"token : {token}");
                var request = new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri($"https://api.spotify.com/v1/artists/{artist.SpotifyId}/top-tracks?market=SE") };
                request.Headers.Add("Authorization", $"Bearer {token}");

                var response = await httpClient.SendAsync(request);

                //Använd ensuresuccessstatuscode istället.
                if (response.IsSuccessStatusCode)
                {
                    {   //uncomment to see song data
                        //await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
                    }

                    Console.WriteLine("Got Response, successfull!!!");

                    string responseData = await response.Content.ReadAsStringAsync()!;
                    TopTracksReponse songs = JsonSerializer.Deserialize<TopTracksReponse>(responseData)!;

                    //Console.WriteLine($"spotifyId : {artist.SpotifyId}");
                    Console.WriteLine($"spotifyTracks : {songs.Tracks.Count}");

                    foreach (Track track in songs.Tracks)
                    {
                        topSongs.Add(new SongDto
                        {
                            //add artists instead of one artist?!
                            Artist = artist.Name,
                            Name = track.name,
                            SpotifyId = track.id
                        });
                    }

                    return topSongs;
                }

                else
                {
                    Console.WriteLine($"not successfull status code : {response.StatusCode}");
                    return topSongs;
                }
            }
        }
    }
}
