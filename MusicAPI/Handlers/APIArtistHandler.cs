using MusicAPI.Data;
using MusicAPI.Models.Dtos;
using MusicAPI.Services;
using System.Net;

namespace MusicAPI.Handlers
{
    public class APIArtistHandler
    {
        public static IResult AddArtist(ArtistDto artistDto, int genreId, IArtistHelper artistHelper)
        {
            artistHelper.AddArtist(artistDto, genreId);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult AddSong(SongDto songDto, int artistId, int genreId, IArtistHelper artistHelper)
        {
            try
            {
                artistHelper.AddSong(songDto, artistId, genreId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to add song {ex.Message}");
            }
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult AddGenre(GenreDto genreDto, IArtistHelper artistHelper)
        {
            artistHelper.AddGenre(genreDto);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult GetArtists(int userId, IArtistHelper artistHelper)
        {
            try
            {
                artistHelper.GetArtists(userId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get artists {ex.Message}");
            }
            return Results.Json(artistHelper.GetArtists(userId));
        }
        public static IResult GetGenres(int userId, IArtistHelper artistHelper)
        {
            try
            {
                artistHelper.GetGenres(userId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest($"Unable to get genres {ex.Message}");
            }
            return Results.Json(artistHelper.GetGenres(userId));
        }
        public static IResult GetSongs(int userId, IArtistHelper artistHelper)
        {
            var songs = artistHelper.GetSongs(userId);
            return Results.Json(songs);
        }
    }
}
