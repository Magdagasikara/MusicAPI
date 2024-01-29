using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Repositories;
using System.Net;
using System.Xml.Linq;

namespace MusicAPI.Handlers
{
    public class APIArtistHandler
    {
        public static IResult AddArtist(ArtistDto artistDto, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.AddArtist(artistDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add Genre {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult AddSong(SongDto songDto, int artistId, int genreId, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.AddSong(songDto, artistId, genreId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add song {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult AddGenre(GenreDto genreDto, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.AddGenre(genreDto);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add Genre {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult GetArtistsForUser(string username, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.GetArtistsForUser(username);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new {Message =$"Unable to get artists: {ex.Message}" });
            }
            return Results.Json(artistRepo.GetArtistsForUser(username));
        }
        public static IResult GetGenresForUser(string username, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.GetGenresForUser(username);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get genres: {ex.Message}");
            }
            return Results.Json(artistRepo.GetGenresForUser(username));
        }
        public static IResult GetSongsForUser(string username, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.GetSongsForUser(username);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get songs: {ex.Message}");
            }
            return Results.Json(artistRepo.GetSongsForUser(username));

        }

        public static IResult GetArtists(HttpContext context, IArtistRepository artistRepo, string? name, int? amountPerPage, int? pageNumber)
        {
            try
            {
                artistRepo.GetArtists(name, amountPerPage, pageNumber);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { Message = $"Unable to get artists: {ex.Message}" });
            }
            return Results.Json(artistRepo.GetArtists(name, amountPerPage, pageNumber));
        }

        public static IResult GetGenres(HttpContext context, IArtistRepository artistRepo, string? title, int? amountPerPage, int? pageNumber)
        {
            try
            {
                artistRepo.GetGenres(title, amountPerPage, pageNumber);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get genres: {ex.Message}");
            }
            return Results.Json(artistRepo.GetGenres(title, amountPerPage, pageNumber));
        }

        public static IResult GetSongs(HttpContext context, IArtistRepository artistRepo, string? name, int? amountPerPage, int? pageNumber)
        {
            try
            {
                artistRepo.GetSongs(name, amountPerPage, pageNumber);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get songs: {ex.Message}");
            }
            return Results.Json(artistRepo.GetSongs(name, amountPerPage, pageNumber));

        }
    }
}
