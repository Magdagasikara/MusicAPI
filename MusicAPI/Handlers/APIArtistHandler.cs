using MusicAPI.Data;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Repositories;
using System.Net;

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
        public static IResult GetArtists(int userId, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.GetArtists(userId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get artists {ex.Message}");
            }
            return Results.Json(artistRepo.GetArtists(userId));
        }
        public static IResult GetGenres(int userId, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.GetGenres(userId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get genres {ex.Message}");
            }
            return Results.Json(artistRepo.GetGenres(userId));
        }
        public static IResult GetSongs(int userId, IArtistRepository artistRepo)
        {
            try
            {
                artistRepo.GetSongs(userId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get songs {ex.Message}");
            }
            return Results.Json(artistRepo.GetSongs(userId));

        }
    }
}
