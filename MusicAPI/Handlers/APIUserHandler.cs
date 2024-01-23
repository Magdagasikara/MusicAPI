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
            try
            {
                var users = userHelper.GetAllUsers();
                return Results.Json(users);
            }
            catch (ArgumentNullException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            

        }
        public static IResult GetUser(int userId, IUserHelper userHelper)
        {
            try
            {
                var user = userHelper.GetUser(userId);
                return Results.Json(user);
            }
            catch(UserNotFoundException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            
        }
        public static IResult AddUser(UserDto user, IUserHelper userHelper)
        {

            if (user.Name == null)
            {
                return Results.BadRequest(new { Message = "User needs to have a name" });
            }

            try
            {
                userHelper.AddUser(user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Exception {ex.Message}");
            }

            return Results.StatusCode((int)HttpStatusCode.Created);

        }
        public static IResult ConnectSongToUser(int userId, int songId, IUserHelper userHelper)
        {
            try
            {
                userHelper.ConnectSongToUser(userId, songId);
            }
            catch (UserNotFoundException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            catch (SongNotFoundException ex)
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
            catch (UserNotFoundException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            catch (ArtistNotFoundException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Exception {ex.Message}");
            }

            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult ConnectGenreToUser(int userId, int genreId, IUserHelper userHelper, IArtistHelper artistHelper)
        {
            try
            {
                userHelper.ConnectGenreToUser(userId, genreId);
            }
            catch (UserNotFoundException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            catch (GenreNotFoundException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Exception {ex.Message}");
            }

            return Results.StatusCode((int)HttpStatusCode.Created);
        }
    }
}
