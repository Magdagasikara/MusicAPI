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

        public static async Task UserMenuOptions()
        {
            while (true)
            {

                string userResponse = Console.ReadLine();

                DisplayUserMenu();

                switch (userResponse)
                {

                    case "1":
                        await UserHandler.GetSongs(loggedInUser);
                        break;

                    case "2":
                        await UserHandler.GetArtists(loggedInUser);
                        break;

                    case "3":
                        await UserHandler.GetGenres(loggedInUser);
                        break;

                    // User
                    // Disclaimer: if song does not exist => redirect to AddSong
                    case "4":
                        await UserHandler.ConnectSongToUser(loggedInUser);
                        break;

                    // User
                    // Disclaimer: if song does not exist => redirect to AddGenre
                    case "5":
                        await UserHandler.ConnectGenreToUser(loggedInUser);
                        break;

                    // User
                    // Disclaimer: if song does not exist => redirect to AddArtist
                    case "6":
                        await UserHandler.ConnectArtistToUser(loggedInUser);
                        break;

                    case "7":
                        return;

                    default:
                        await Console.Out.WriteLineAsync("Invalid input, try again.");
                        break;

                }
            }
        }

        static void DisplayUserMenu()
        {
            Console.WriteLine("Welcome to Music API! Please enter:");
            Console.WriteLine("-----------------------------------_");
            Console.WriteLine("1. To view all users");
            Console.WriteLine("2. To view specific user");
            Console.WriteLine("3. To add user");
            Console.WriteLine("4. To add song");
            Console.WriteLine("5. To add genre");
            Console.WriteLine("6. To add artist");
            Console.WriteLine("7. To add song to user");
            Console.WriteLine("8. To add genre to user");
            Console.WriteLine("9. To add artist to user");
            Console.WriteLine("10. To view all user's songs");
            Console.WriteLine("11. To view all user's artists");
            Console.WriteLine("12. To view all user's genres");
            Console.WriteLine("13. To exit.");
        }
    }
}
