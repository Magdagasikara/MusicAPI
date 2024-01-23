using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MusicAPI.Handlers
{
    public interface IAPISpotifyHandler
    {
        Task<string> GetAcessToken();
        
    }
    public class APISpotifyHandler
    {
        private string? _acessToken;
        private HttpClient _httpClient;
        private string _clientId;
        private string _clientSecret;
        private DateTime _lastUpdatedToken;

        public APISpotifyHandler(string clientId, string clientSecret) : this(new HttpClient(), clientId, clientSecret)
        {
            
        }

        public APISpotifyHandler(HttpClient httpClient, string clientId, string clientSecret)
        {
            _httpClient = httpClient;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        
    }
}
