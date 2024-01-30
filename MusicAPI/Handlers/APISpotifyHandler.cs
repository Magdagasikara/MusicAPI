using MusicAPI.Services;
using System.Net;

namespace MusicAPI.Handlers
{
    public class APISpotifyHandler
    {
        public static async Task<IResult> AddArtistGenreAndTracksFromSpotify(string searchArtist, ISpotifyHelper spotifyHelper)
        {
            try
            {
                await spotifyHelper.SaveArtistGenreAndTrackFromSpotifyToDb(searchArtist);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add items from searchArtist to database {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
