using MusicAPI.Services;
using MusicAPIClient.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAPIClient.MenuOptions
{
    public class AdminMenu
    {
        public static async Task AdminMenuOptions(HttpClient client, string username)
        {

            while (true)
            {
                await DisplayAdminMenu(username);
                string userResponse = Console.ReadLine();

                switch (userResponse)
                {
                    // Admin
                    case "1":
                        await AdminHandler.GetAllUsers(client);
                        break;

                    case "2":
                        await AdminHandler.AddUser(client);
                        break;

                    case "3":
                        await AdminHandler.AddSong(client);
                        break;

                    case "4":
                        await AdminHandler.AddGenre(client);
                        break;

                    case "5":
                        await AdminHandler.AddArtist(client);
                        break;

                    case "X":
                    case "x":
                        return;

                    default:
                        await Console.Out.WriteLineAsync("Invalid input, try again.");
                        break;
                }
            }
        }

        static async Task DisplayAdminMenu(string username)
        {
            Console.WriteLine($"Welcome to Music API {username}! Please enter:");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("1. To view all users");
            Console.WriteLine("2. To add new user");
            Console.WriteLine("3. To add new song");
            Console.WriteLine("4. To add new genre");
            Console.WriteLine("5. To add new artist");
            Console.WriteLine("X. To exit");
        }
    }
}
