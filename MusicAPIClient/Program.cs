using MusicAPI.Services;
using MusicAPIClient.Handlers;
using MusicAPIClient.APIModels;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using MusicAPIClient.MenuOptions;
using System.Text;
using MusicAPI.Handlers;
using MusicAPI.Repositories;
using MusicAPI.Services;

namespace MusicAPIClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7181");

                ISpotifyHelper spotifyHelper = new SpotifyHelper(client);
                IArtistRepository artistRepository = new DbArtistRepository(client);

                await LogIn.LogInUser(client, spotifyHelper, artistRepository);
            }
        }
    }
}

