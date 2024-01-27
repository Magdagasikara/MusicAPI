using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Handlers;
using MusicAPI.Models;
using MusicAPI.Models.Dtos;
using MusicAPI.Repositories;
using MusicAPI.Services;

namespace MusicAPI
{
    public class Program
    {
        public static List<ArtistDto> artistDtosForDB { get; private set; }

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IArtistRepository, DbArtistRepository>();
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
            /* ------------------------KOMMENTERAT UT PGA ERROR------------------------------------------------------
            app.MapGet("/user/", APIUserHandler.GetAllUsers);
            app.MapGet("/user/{userId}", APIUserHandler.GetUser);
            --------------------------------------------------------------------------------------------------------
            */



            // skapa liknande 3 metoder i UserRepo som heter ist�llet "GetArtistForUser" etc?
            app.MapGet("/artist/", APIArtistHandler.GetArtists);
            app.MapGet("/genre/", APIArtistHandler.GetGenres);
            app.MapGet("/song/", APIArtistHandler.GetSongs);

            // POSTS - user
            /* ------------------------KOMMENTERAT UT F�R ERROR------------------------------------------------------
            app.MapPost("/user/", APIUserHandler.AddUser);
            app.MapPost("/user/{userId}/song/{songId}", APIUserHandler.ConnectSongToUser);
            app.MapPost("/user/{userId}/artist/{artistId}", APIUserHandler.ConnectArtistToUser);
            app.MapPost("/user/{userId}/genre/{genreId}", APIUserHandler.ConnectGenreToUser);
            --------------------------------------------------------------------------------------------------------
            */


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

            
            //used to populate database with top songs from the artists of the top 100 most streamed songs on spotify.
            List <ArtistDto> top100ArtistsForDb = new List<ArtistDto>();
            int offset = 0;
            for (int i = 1; i <= 2; i++)
            {
                List<ArtistDto>? artistDtosForDB = await spotifyHelper.GetTopAllTime100(50, offset, token);

                foreach (var art in artistDtosForDB)
                {
                    if (!top100ArtistsForDb.Any(a => a.SpotifyId == art.SpotifyId) )
                    {
                        top100ArtistsForDb.Add(art);
                    }
                }
                offset = 50;
            }

            List<SongDto> songsForDB = new List<SongDto>();
            foreach (ArtistDto artist in top100ArtistsForDb)
            {
                //Add each Artist to DB.

                List<SongDto>? topSongsByArtist = await spotifyHelper.GetTopTracksFromArtist(artist, token);

                //check if song allready exists in list by spotify ID??
                //
            }

            //await Console.Out.WriteLineAsync(xx);


            app.Run();
        }
    }
}
