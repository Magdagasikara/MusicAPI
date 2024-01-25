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
            builder.Services.AddHttpClient<ISpotifyAccountHelper, SpotifyAccountHelper>(c => { c.BaseAddress = new Uri("https://accounts.spotify.com/api/"); });
            builder.Services.AddHttpClient<ISpotifyHelper, SpotifyHelper>(c => { c.BaseAddress = new Uri("https://api.spotify.com/vi/"); c.DefaultRequestHeaders.Add("Accept", "application/.json"); });

            var app = builder.Build();

            app.MapGet("/", () => "Hello klassen!");

            // GETS - artists/songs/genres
            //ändra dessa 3 metoder så de bara returnerar artists utan userId?
            //app.MapGet("/artist/", APIArtistHandler.GetArtists);
            //app.MapGet("/genre/", APIArtistHandler.GetGenres);
            //app.MapGet("/song/", APIArtistHandler.GetSongs);

            // POSTS - artists/songs/genres
            app.MapPost("/artist/", APIArtistHandler.AddArtist);
            app.MapPost("/genre/", APIArtistHandler.AddGenre);
            app.MapPost("/song/", APIArtistHandler.AddSong);

            // GETS - user
            app.MapGet("/user/", APIUserHandler.GetAllUsers);
            app.MapGet("/user/{userId}", APIUserHandler.GetUser);
            // skapa liknande 3 metoder i UserRepo som heter istället "GetArtistForUser" etc?
            app.MapGet("/artist/", APIArtistHandler.GetArtists);
            app.MapGet("/genre/", APIArtistHandler.GetGenres);
            app.MapGet("/song/", APIArtistHandler.GetSongs);

            // POSTS - user
            app.MapPost("/user/", APIUserHandler.AddUser);
            app.MapPost("/user/{userId}/song/{songId}", APIUserHandler.ConnectSongToUser);
            app.MapPost("/user/{userId}/artist/{artistId}", APIUserHandler.ConnectArtistToUser);
            app.MapPost("/user/{userId}/genre/{genreId}", APIUserHandler.ConnectGenreToUser);

            app.Run();
        }
    }
}
