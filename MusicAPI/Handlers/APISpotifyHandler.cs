using MusicAPI.Services;
using System.Net;

namespace MusicAPI.Handlers
{
    public class APISpotifyHandler
    {
      
        public static async Task<IResult> AddArtistGenreAndTracksFromSpotify(string searchArtist, string description, ISpotifyHelper spotifyHelper)

        {
            try
            {
                await spotifyHelper.SaveArtistGenreAndTrackFromSpotifyToDb(searchArtist, description);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add items from searchArtist to database {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }

        public static async Task<IResult> Top100MostFollowedArtistsTop10Songs(ISpotifyHelper spotifyHelper)

        {
            try
            {
                await spotifyHelper.GetTop100MostPopularArtists();
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add items from Playlist to database {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
