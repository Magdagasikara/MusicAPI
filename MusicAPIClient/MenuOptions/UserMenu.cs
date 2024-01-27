using MusicAPI.Services;
using MusicAPIClient.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAPIClient.MenuOptions
{
    public class UserMenu
    {

        public static async Task UserMenuOptions(HttpClient client, string username)
        {
            while (true)
            {

                DisplayUserMenu(username);
                string userResponse = Console.ReadLine();


                switch (userResponse)
                {

                    case "1":
                        await UserHandler.GetArtistsForUser(client, username);
                        break;

                    case "2":
                        //await UserHandler.GetSongs(userId);
                        break;

                    case "3":
                        //await UserHandler.GetGenres(userId);
                        break;

                    // User
                    // Disclaimer: if song does not exist => redirect to AddSong
                    case "4":
                        //await UserHandler.ConnectArtistToUser(userId);
                        break;

                    // User
                    // Disclaimer: if song does not exist => redirect to AddGenre
                    case "5":
                        //await UserHandler.ConnectSongToUser(userId);
                        break;

                    // User
                    // Disclaimer: if song does not exist => redirect to AddArtist
                    case "6":
                        //await UserHandler.ConnectGenreToUser(userId);                        
                        break;

                    case "x":
                    case "X":
                        return;

                    default:
                        //await Console.Out.WriteLineAsync("Invalid input, try again.");
                        break;

                }
            }
        }

        static void DisplayUserMenu(string username)
        {
            Console.Clear();
            Console.WriteLine($"Welcome to Music API, {username}! Please enter:");
            Console.WriteLine("-----------------------------------_");
            Console.WriteLine("1. To view all artists you like");
            Console.WriteLine("2. To view all songs you like");
            Console.WriteLine("3. To view all genres you like");
            Console.WriteLine("4. To add a new artist to your collection");
            Console.WriteLine("5. To add a new song to your collection");
            Console.WriteLine("6. To add a new genre to your collection");
            Console.WriteLine("X. To exit.");
        }
    }
}
