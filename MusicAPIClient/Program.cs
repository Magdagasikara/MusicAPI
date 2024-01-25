using MusicAPI.Services;
using MusicAPIClient.APIModels;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MusicAPIClient
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7181/");

                var loggedInUser = LogInUser(userHelper);

            while (true)
            {
                DisplayMenu();

                string userResponse = Console.ReadLine();

                switch (userResponse)
                {
                        // These methods will be changed 
                    case "1":
                        await .GetAllUsers(loggedInUser);
                        break;

                    case "2":
                        await userHelper.GetUser(loggedInUser);
                        break;

                    case "3":
                        await userHelper.AddUser(loggedInUser);
                        break;

                    case "4":
                        await artistHelper.AddSong(loggedInUser);
                        break;

                    case "5":
                        await artistHelper.AddGenre(loggedInUser);
                        break;

                    case "6":
                        await artistHelper.AddArtist(loggedInUser);
                        break;

                    case "7":
                        await userHelper.ConnectSongToUser(loggedInUser);
                        break;

                    case "8":
                        await userHelper.ConnectGenreToUser(loggedInUser);
                        break;

                    case "9":
                        await userHelper.ConnectArtistToUser(loggedInUser);
                        break;

                    case "10":
                        await artistHelper.GetSongs(loggedInUser);
                        break;

                    case "11":
                        await artistHelper.GetArtists(loggedInUser);
                        break;

                    case "12":
                        await artistHelper.GetGenres(loggedInUser);
                        break;

                    case "13":
                        return;

                    default:
                        await Console.Out.WriteLineAsync("Invalid input, try again.");
                        break;

                }
            }
        }

        static void DisplayMenu()
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

        static User LogInUser(UserHelper userHelper)
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            // Logic to check if user already exists
            // return loggedInUser;
        }
    }
}

