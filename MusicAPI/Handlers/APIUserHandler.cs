using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Repositories;
using System.Net;
using System.Text;

namespace MusicAPI.Handlers
{
    public static class APIUserHandler
    {
        public static IResult GetAllUsers(IUserRepository userRepo)
        {
            try
            {
                var users = userRepo.GetAllUsers();
                return Results.Json(users);
            }
            catch (ArgumentNullException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }
        }

        public static IResult GetUser(string username, IUserRepository userRepo)
        {
            try
            {
                var user = userRepo.GetUser(username);
                return Results.Json(user);
            }
            catch(UserNotFoundException ex)
            {
                return Results.NotFound($"Exception {ex.Message}");
            }       
        }

        public static IResult AddUser(UserDto user, IUserRepository userRepo)
        {

            if (user.Name == null)
            {
                return Results.BadRequest(new { Message = "User needs to have a name" });
            }

            try
            {
                userRepo.AddUser(user);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Exception {ex.Message}");
            }

            return Results.StatusCode((int)HttpStatusCode.Created);

        }

        public static IResult ConnectSongToUser(string username, int songId, IUserRepository userRepo)
        {
            try
            {
                userRepo.ConnectSongToUser(username, songId);
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

        public static IResult ConnectArtistToUser(string username, int artistId, IUserRepository userRepo)
        {
            try
            {
                userRepo.ConnectArtistToUser(username, artistId);
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

        public static IResult ConnectGenreToUser(string username, int genreId, IUserRepository userHelper, IArtistRepository artistRepo)
        {
            try
            {
                userHelper.ConnectGenreToUser(username, genreId);
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
