using System.Net.Http.Headers;
using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using MusicAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MusicAPI.Services
{
    public interface ISpotifyHelper
    {
        Task<string> GetToken(string clientId, string clientSecret);
        Task<string> TryGetSthFromSpotify(string token);
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
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

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
    }

}
