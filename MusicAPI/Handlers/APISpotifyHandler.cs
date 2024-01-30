using MusicAPI.Services;
using System.Net;

namespace MusicAPI.Handlers
{
    public class APISpotifyHandler
    {
        public static IResult AddArtistGenreAndTracksFromSpotify(string searchArtist, ISpotifyHelper spotifyHelper)
        {
            try
            {
                spotifyHelper.SaveArtistGenreAndTrackFromSpotifyToDb(searchArtist);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add items from searchArtist to database {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
