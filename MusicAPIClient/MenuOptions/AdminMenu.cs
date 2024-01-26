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
        public static async Task AdminMenuOptions()
        {

            while (true)
            {
                string userResponse = Console.ReadLine();
                DisplayAdminMenu();

                switch (userResponse)
                {
                    // Admin
                    case "1":
                        await AdminHandler.GetAllUsers(client);
                        break;

                    case "2":
                        await UserHandler.AddUser(loggedInUser);
                        break;

                    case "3":
                        await AdminHandler.AddSong(loggedInUser);
                        break;

                    case "4":
                        await AdminHandler.AddGenre(loggedInUser);
                        break;

                    case "5":
                        await AdminHandler.AddArtist(loggedInUser);
                        break;

                    case "6":
                        return;

                    default:
                        await Console.Out.WriteLineAsync("Invalid input, try again.");
                        break;

                }
            }
        }

        static void DisplayAdminMenu()
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
