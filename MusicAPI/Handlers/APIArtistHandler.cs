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
        public static IResult AddSong(SongDto songDto, int artistId, IArtistHelper artistHelper)
        {
            artistHelper.AddSong(songDto, artistId);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult AddGenre(GenreDto genreDto, IArtistHelper artistHelper)
        {
            artistHelper.AddGenre(genreDto);
            return Results.StatusCode((int)HttpStatusCode.Created);
        }
        public static IResult GetArtists(int userId, IArtistHelper artistHelper)
        {
            var artists = artistHelper.GetArtists(userId);
            return Results.Json(artists);
        }
        public static IResult GetGenres(int userId, IArtistHelper artistHelper)
        {
            var genres = artistHelper.GetGenres(userId);
            return Results.Json(genres);
        }
        public static IResult GetSongs(int userId, IArtistHelper artistHelper)
        {
            var songs = artistHelper.GetSongs(userId);
            return Results.Json(songs);
        }
    }
}
