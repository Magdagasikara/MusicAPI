using MusicAPI.Services;
using MusicAPIClient.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPIClient.Helpers;

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
                        LogIn.LogOutUser();
                        await LogIn.LogInUser(client);
                        break;

                    default:
                        await Console.Out.WriteLineAsync("Invalid input, try again.");
                        break;
                }
            }
        }

        static async Task DisplayAdminMenu(string username)
        {
            Console.Clear();
            MenuHelper.HeaderUserAdmin(username);
            Console.WriteLine("1. To view all users");
            Console.WriteLine("2. To add new user");
            Console.WriteLine("3. To add new song");
            Console.WriteLine("4. To add new genre");
            Console.WriteLine("5. To add new artist");

            // STINA
            // Console.WriteLine("6. Add songs for top 100 (kolla med Stina beskrivning) artists");
            // -- när success: Successfully Added Tracks, Artists and Genres
            // app.MapGet("/spotify/top100/", nån-spotify-handler.StinaMetod utan nån mer input som returnerar IResult)

            // LUKAS
            // Console.WriteLine("7. Add 50 songs for a chosen artist");
            // -- när success: Successfully Added Tracks, Artist and Genre
            // app.MapGet("/spotify/{artist}/", nån-spotify-handler.LukasMetod som använder ditt artistnamn och returnerar IResult)


            Console.WriteLine("X. Log out");
        }
    }
}
