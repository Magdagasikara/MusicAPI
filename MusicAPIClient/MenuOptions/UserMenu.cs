using MusicAPI.Repositories;
using MusicAPI.Services;
using MusicAPIClient.Handlers;
using MusicAPIClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MusicAPIClient.MenuOptions
{
    public class UserMenu
    {

        public static async Task UserMenuOptions(HttpClient client, string username, ISpotifyHelper spotifyHelper, IArtistRepository artistRepository)
        {
            while (true)
            {

                await DisplayUserMenu(username);
                string userResponse = Console.ReadLine();

                switch (userResponse)
                {

                    case "1":
                        await UserHandler.GetArtistsForUser(client, username);
                        break;

                    case "2":
                        await UserHandler.GetSongsForUser(client, username);
                        break;

                    case "3":
                        await UserHandler.GetGenresForUser(client, username);
                        break;

                    case "4":
                        await UserHandler.ConnectArtistToUser(client, username);
                        break;

                    case "5":
                        await UserHandler.ConnectSongToUser(client, username);
                        break;

                    case "6":
                        await UserHandler.ConnectGenreToUser(client, username);                        
                        break;

                    case "x":
                    case "X":
                        LogIn.LogOutUser();
                        await LogIn.LogInUser(client, spotifyHelper, artistRepository);
                        break;

                    default:
                        await Console.Out.WriteAsync("Invalid input, try again.");
                        break;
                }
            }
        }

        static async Task DisplayUserMenu(string username)
        {
            Console.Clear();
            MenuHelper.HeaderUserAdmin(username);
            await Console.Out.WriteLineAsync("1. To view all artists you like");
            await Console.Out.WriteLineAsync("2. To view all songs you like");
            await Console.Out.WriteLineAsync("3. To view all genres you like");
            await Console.Out.WriteLineAsync("4. To add a new artist to your collection");
            await Console.Out.WriteLineAsync("5. To add a new song to your collection");
            await Console.Out.WriteLineAsync("6. To add a new genre to your collection");
            await Console.Out.WriteLineAsync("X. Log out");
        }
    }
}
