using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Handlers;
using MusicAPI.Repositories;
using MusicAPI.Services;
using Moq;

namespace MusicAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            builder.Services.AddScoped<IArtistRepository, DbArtistRepository>();
            builder.Services.AddScoped<IUserRepository, DbUserRepository>();
            builder.Services.AddHttpClient<ISpotifyHelper, SpotifyHelper>(c => { c.BaseAddress = new Uri("https://api.spotify.com/v1/"); c.DefaultRequestHeaders.Add("Accept", "application/.json"); });
            builder.Services.AddHttpClient<ISpotifyAccountHelper, SpotifyAccountHelper>(c => { c.BaseAddress = new Uri("https://accounts.spotify.com/api/"); });

            var app = builder.Build();

            app.MapGet("/", () => "Välkommen till vår presentation klassen :)");

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

            // POSTS - spotify
            app.MapPost("spotify/search/{searchArtist}", APISpotifyHandler.AddArtistGenreAndTracksFromSpotify);

            //var contextOptions = new DbContextOptionsBuilder<ApplicationContext>()
            //  .UseSqlServer(connectionString)
            //    .Options;

            //var context = new ApplicationContext(contextOptions);

            //var httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri("https://accounts.spotify.com/api/");

            //ISpotifyAccountHelper spotifyAccountHelper = new SpotifyAccountHelper(httpClient);

            //IConfiguration configuration = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.Development.json")
            //    .Build();

            //IArtistRepository artistRepository = new DbArtistRepository(context);

            //var spotifyHelper = new SpotifyHelper(httpClient, spotifyAccountHelper, configuration, artistRepository);

            //await spotifyHelper.SaveArtistGenreAndTrackFromSpotifyToDb("d");

            //await Console.Out.WriteLineAsync("Successfully Added Tracks, Artist and Genre");

            app.Run();
        }
    }
}
