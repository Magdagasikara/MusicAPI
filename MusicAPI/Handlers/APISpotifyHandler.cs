using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using MusicAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;
using MusicAPI.Models.Dtos;
using Microsoft.AspNetCore.Mvc.Filters;

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


        // fix so that methods run via handlers //STina
        //public async Task<IResult> AddTop100ArtistsWithTop10Tracks(string token)
        //{
        //    try
        //    {
        //        await new ISpotifyHelper.GetTop100StreamedArtists(token);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Results.BadRequest(ex);
        //    }

        //    return Results.StatusCode((int)HttpStatusCode.Created);
        //}
    }
}
