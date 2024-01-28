using MusicAPI.Services;
using MusicAPIClient.Handlers;
using MusicAPIClient.APIModels;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using MusicAPIClient.MenuOptions;
using System.Text;

namespace MusicAPIClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // In program user logs in => checks if user is admin or regular
            // => if user is regular: UserMenu() => is user is admin: AdminMenu()
            // If user does not exist => create new user (UserHandler.AddUser) 
            // => redirects to UserMenu()
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7181");
                await LogIn.LogInUser(client);
            }
        }
    }
}

