using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Handlers;
using MusicAPI.Repositories;
using MusicAPI.Services;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Primitives;

namespace MusicAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            //�ndra dessa 3 metoder s� de bara returnerar artists utan userId?
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
            // skapa liknande 3 metoder i UserRepo som heter ist�llet "GetArtistForUser" etc?
            app.MapGet("/artist/", APIArtistHandler.GetArtists);
            app.MapGet("/genre/", APIArtistHandler.GetGenres);
            app.MapGet("/song/", APIArtistHandler.GetSongs);

            // POSTS - user
            app.MapPost("/user/", APIUserHandler.AddUser);
            app.MapPost("/user/{userId}/song/{songId}", APIUserHandler.ConnectSongToUser);
            app.MapPost("/user/{userId}/artist/{artistId}", APIUserHandler.ConnectArtistToUser);
            app.MapPost("/user/{userId}/genre/{genreId}", APIUserHandler.ConnectGenreToUser);


            // testa mer era clientId och clientSecret
            string clientId = "";
            string clientSecret = "";

            // h�r h�mtar jag min token och skriver ut den i konsolen f�r att anv�nda den i Insomnia
            // den ska in under Auth-Bearer Token
            var spotifyHelper = app.Services.GetRequiredService<ISpotifyHelper>();
            string token = await spotifyHelper.GetToken(clientId, clientSecret);
            await Console.Out.WriteLineAsync(token);

            // h�r testar jag med en av tests�kv�gar f�r Spotify
            // den �r h�rdkodad i metoden just nu och returnerar json som string, bara som test
            // skriver ut den i konsolen f�r att se att den funkat
            string xx = await spotifyHelper.TryGetSthFromSpotify(token);
            await Console.Out.WriteLineAsync(xx);

            app.Run();

        }
    }
}
