using MusicAPI.Services;

namespace MusicAPI.Handlers
{
    public class APITicketmasterHandler
    {
        public static async Task<IResult> GetConcertsForArtist(string searchArtist, ITicketmasterHelper ticketmasterHelper)
        {
            try
            {
                await ticketmasterHelper.ReturnConcertsForArtistAsync(searchArtist);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { Message = $"{ex.Message}" });
            }
            return Results.Json(await ticketmasterHelper.ReturnConcertsForArtistAsync(searchArtist));
        }
    }
}
