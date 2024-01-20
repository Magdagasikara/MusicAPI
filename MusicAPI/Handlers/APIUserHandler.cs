using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Services;
using System.Net;
using System.Text;

namespace MusicAPI.Handlers
{
    public static class APIUserHandler
    {
        public static IResult GetAllUsers(IUserHelper userHelper)
        {
            var users = userHelper.GetAllUsers();
            return Results.Json(users);
        }
        public static IResult GetUser(int userId, IUserHelper userHelper)
        {
            var user = userHelper.GetUser(userId);
            return Results.Json(user);
        }
        public static IResult AddUser(UserDto user, IUserHelper userHelper)
        {

            if (user.Name == null)
            {
                return Results.BadRequest(new { Message = "User needs to have a name" });
            }

            userHelper.AddUser(user);

            return Results.StatusCode((int)HttpStatusCode.Created);

        }
        public static IResult ConnectSongToUser(int userId, int songId, IUserHelper userHelper, IArtistHelper artistHelper)
        {
            try
            {
                userHelper.ConnectSongToUser(userId, songId);
            }
            catch (ArgumentNullException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Exception {ex.Message}");
            }

            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ConnectArtistToUser(int userId, int artistId, IUserHelper userHelper)
        {
            try
            {
                userHelper.ConnectArtistToUser(userId, artistId);
            }
            catch (ArgumentNullException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Exception {ex.Message}");
            }

            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ConnectGenreToUser(int userId, int genreId, IUserHelper userHelper)
        {
            userHelper.ConnectGenreToUser(userId, genreId);
            return Results.StatusCode((int)HttpStatusCode.Created);

        }
    }
}
