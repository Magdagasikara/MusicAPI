using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Handlers;
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
            builder.Services.AddScoped<IArtistHelper, ArtistHelper>();
            builder.Services.AddHttpClient<ISpotifyHelper, SpotifyHelper>(c =>
            {
                c.BaseAddress = new Uri("https://accounts.spotify.com/api/");
            });

            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.MapGet("/getartists/{userId}", APIArtistHandler.GetGenres);

            app.Run();
        }
    }
}
