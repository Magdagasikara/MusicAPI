using Microsoft.AspNetCore.Mvc;
using MusicAPI.Services;
using System.Net;
using System.Net.Http.Headers;

namespace MusicAPI.Handlers
{
    public class APISpotifyHandler
    {
        private readonly ISpotifyAccountHelper _spotifyAccountHelper;
        private readonly IConfiguration _configuration;
        private readonly ISpotifyHelper _spotifyHelper;
        public APISpotifyHandler(ISpotifyAccountHelper spotifyAccountHelper, IConfiguration configuration, ISpotifyHelper spotifyHelper)
        {
            _spotifyAccountHelper = spotifyAccountHelper;
            _configuration = configuration;
            _spotifyHelper = spotifyHelper;
        }

        public async Task<IResult> PopulateDatabaseFromSpotify(string searchQuery)
        {
            try
            {
                var token = await _spotifyAccountHelper.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);

                var getArtistGenreAndTrack = await _spotifyHelper.GetArtistGenreAndTrack(token, searchQuery);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Exception {ex.Message}");
            }

            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
