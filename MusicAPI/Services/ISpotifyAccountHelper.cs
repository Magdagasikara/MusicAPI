using System.Net.Http.Headers;
using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using MusicAPI.Models;

namespace MusicAPI.Services
{
    public interface ISpotifyAccountHelper
    {
        Task<string> GetToken(string clientId, string clientSecret);
    }

    public class SpotifyAccountHelper : ISpotifyAccountHelper
    {
        private readonly HttpClient _httpClient;

        public SpotifyAccountHelper(HttpClient httpClient)
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

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(responseStream);

            return authResult.access_token;
        }
    }

}
