using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Handlers;
using MusicAPI.Repositories;
using MusicAPI.Services;

namespace MusicAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IArtistRepository, DbArtistRepository>();
            builder.Services.AddScoped<IUserRepository, DbUserRepository>();
            builder.Services.AddHttpClient<ISpotifyHelper, SpotifyHelper>(c =>
            {
                c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
            });

            var app = builder.Build();

            app.MapGet("/", () => "Hello klassen!");

            // GETS - artists/songs/genres
            app.MapGet("/artist/", APIArtistHandler.GetArtists);
            app.MapGet("/genre/", APIArtistHandler.GetGenres);
            app.MapGet("/song/", APIArtistHandler.GetSongs);

            // POSTS - artists/songs/genres
            app.MapPost("/artist/", APIArtistHandler.AddArtist);
            app.MapPost("/genre/", APIArtistHandler.AddGenre);
            app.MapPost("/song/{artistId}/{genreId}", APIArtistHandler.AddSong);

            // GETS - user
            app.MapGet("/user/", APIUserHandler.GetAllUsers);
            app.MapGet("/user/{username}", APIUserHandler.GetUser);
            app.MapGet("/user/{username}/artist/", APIArtistHandler.GetArtistsForUser);
            app.MapGet("/user/{username}/genre/", APIArtistHandler.GetGenresForUser);
            app.MapGet("/user/{username}/song/", APIArtistHandler.GetSongsForUser);

            // POSTS - user
            app.MapPost("/user/", APIUserHandler.AddUser);
            app.MapPost("/user/{username}/song/{songId}", APIUserHandler.ConnectSongToUser);
            app.MapPost("/user/{username}/artist/{artistId}", APIUserHandler.ConnectArtistToUser);
            app.MapPost("/user/{username}/genre/{genreId}", APIUserHandler.ConnectGenreToUser);

            app.Run();
        }
    }
}
