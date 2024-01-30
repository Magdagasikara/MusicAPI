using ConsoleTables;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MusicAPI.Migrations;
using MusicAPI.Models.ViewModel;
using MusicAPIClient.APIModels;
using MusicAPIClient.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicAPIClient.Handlers
{
    // Magda
    public class UserHandler
    {
        public static async Task GetArtistsForUser(HttpClient client, string username)
        {

            // OBS lägg till omredigering från BadRequest
            // "vill du lägga till artister nu?" och skicka till den metoden

            HttpResponseMessage response = await client.GetAsync($"/user/{username}/artist/");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await Console.Out.WriteLineAsync("No saved artists yet. Press any key to return to menu.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync($"{response.StatusCode}");
                Console.ReadKey();
                throw new Exception($"Failed to get your saved artists. Status code: {response.StatusCode}");
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();

                ListArtistsWithId[] artists = JsonSerializer.Deserialize<ListArtistsWithId[]>(content);

                var table = new ConsoleTable("ARTIST NAME", "DESCRIPTION");

                foreach (var artist in artists)
                {
                    table.AddRow(artist.Name, artist.Description);
                }
                table.Write(Format.Minimal);
                await Console.Out.WriteLineAsync("Press X to go back to previous menu.");
                Console.ReadKey();
            }
        }

        public static async Task GetSongsForUser(HttpClient client, string username)
        {
            // OBS lägg till omredigering från BadRequest
            // "vill du lägga till låtar nu?" och skicka till den metoden

            HttpResponseMessage response = await client.GetAsync($"/user/{username}/song/");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await Console.Out.WriteLineAsync("No saved songs yet. Press any key to return to menu.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get your saved songs. Status code: {response.StatusCode}");
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();

                ListSongsWithId[] songs = JsonSerializer.Deserialize<ListSongsWithId[]>(content);

                var table = new ConsoleTable("SONG", "ARTIST", "GENRE");

                foreach (var song in songs)
                {
                    table.AddRow(song.Name, song.Artist, song.Genre);
                }
                table.Write(Format.Minimal);
                await Console.Out.WriteLineAsync("Press X to go back to previous menu.");
                Console.ReadKey();
            }
        }

        public static async Task GetGenresForUser(HttpClient client, string username)
        {

            // OBS lägg till omredigering från BadRequest
            // "vill du lägga till genres nu?" och skicka till den metoden

            HttpResponseMessage response = await client.GetAsync($"/user/{username}/genre/");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await Console.Out.WriteLineAsync("No saved genres yet. Press any key to return to menu.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get your saved genres. Status code: {response.StatusCode}");
            }
            else
            {
                string content = await response.Content.ReadAsStringAsync();

                ListGenresWithId[] genres = JsonSerializer.Deserialize<ListGenresWithId[]>(content);

                var table = new ConsoleTable("GENRE");

                foreach (var genre in genres)
                {
                    table.AddRow(genre.Title);
                }
                table.Write(Format.Minimal);
                await Console.Out.WriteLineAsync("Press X to go back to previous menu.");
                Console.ReadKey();
            }
        }

        public static async Task ConnectSongToUser(HttpClient client, string username)
        {
            await Console.Out.WriteLineAsync("Which song do you wish to add to your collection?");

            // 2 scenarier
            // 1:
            // - ange bara låtnamn
            // - söka igenom Db efter låtnamnet
            // -- om den finns: visa, är det låten + artisten + genre som det handlar om?
            // --- Ja: lägg till
            // --- Nej: be user ange artist och genre för att lägga till? hämta från Spotify alla som finns med det namnet?
            // ---- OBS hantera om artist eller genre inte finns eller finns?
            // -- om den inte finns: be user anger artist och genre för att lägga till
            // ---- OBS hantera om artist eller genre inte finns eller finns???

            // 2:
            // - ange låtnamn, artist, genre

            // Disclaimer: if song does not exist => redirect to AddGenre

            // Amandas förslag

            // 1. Method returns available songs in database with songId
            // 2. User chooses song to connect with via songId
            // 3. Connection occurs or exception

            HttpResponseMessage response = await client.GetAsync($"/song/");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await Console.Out.WriteLineAsync("No available songs. Press any key to return to menu and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get songs. Status code: {response.StatusCode}");
            }
            string content = await response.Content.ReadAsStringAsync();

            ListSongsWithId[] songs = JsonSerializer.Deserialize<ListSongsWithId[]>(content);
            int amount = songs.Count();

            Console.WriteLine("Available genres:");
            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine($"{i}. {songs[i].Name}");
            }

            Console.WriteLine("Choose a SongId:");
            int selectedSongId;

            if (!int.TryParse(Console.ReadLine(), out selectedSongId) || selectedSongId < 0 || selectedSongId >= amount)
            {
                Console.WriteLine("Invalid genre number.");
                return;
            }

            // Connecting user's choosen id with song 
            int songId = songs[selectedSongId].Id;

            ConnectUserToSong connectUserToSong = new ConnectUserToSong()
            {
                Username = username,
                SongId = songId
            };

            string json = JsonSerializer.Serialize(connectUserToSong);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage connectResponse = await client.PostAsync($"/user/{username}/song/{songId}/", jsonContent);

            if (connectResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully connected {username} to song!");
            }
            else
            {
                Console.WriteLine($"Failed to connect {username} to song.");
            }
        }

        public static async Task ConnectArtistToUser(HttpClient client, string username)
        {
            // Connect Artist To User
            // Part 1 checks if there are any artists to connect to 
            // Part 2 choose an artist to connect on this on subsequent pages
            // Part 3 connect the artist to user

            int pageNumber = 1;
            int amountPerPage = 10;


            // Part 1

            Console.Clear();
            ConsoleHelper.PrintColorGreen("To see available artists choose: ");
            ConsoleHelper.PrintColorGreen("--------------------------------");
            await Console.Out.WriteLineAsync("1. To see all artists in alphabetical order");
            await Console.Out.WriteLineAsync("2. To search for an artist name");
            await Console.Out.WriteLineAsync("X. To go back to previous menu");

            string input = Console.ReadLine();
            string request = "";
            string nameSearch = "";

            while (true)
            {
                switch (input)
                {
                    case "1":
                        request = $"/artist/";
                        break;

                    case "2":
                        await Console.Out.WriteAsync("Type first letters of the artist name you wish to add: ");
                        nameSearch = Console.ReadLine();
                        request = $"/artist/?name={nameSearch}";
                        break;

                    case "x":
                    case "X":
                        return;

                    default:
                        ConsoleHelper.PrintColorRed("Invalid input, try again.");
                        break;

                }

                if (request != "") break;
            }

            // Counts amount of pages needed 
            // This is made in a separate step only to show we can use pagination in our API
            // Otherwise we would get all of them and show in groups of 10 from result
            HttpResponseMessage response = await client.GetAsync(request);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await Console.Out.WriteLineAsync("No available artists. Press any key to return to menu and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                ConsoleHelper.PrintColorRed($"Failed to get artists. Status code: {response.StatusCode}");
            }
            string content = await response.Content.ReadAsStringAsync();
            ListArtistsWithId[] artists = JsonSerializer.Deserialize<ListArtistsWithId[]>(content);
            int amount = artists.Count();
            int numberOfPages = (int)Math.Ceiling((decimal)amount / amountPerPage);
            if (amount == 0)
            {
                await Console.Out.WriteLineAsync("No available artists. Press a key and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }


            // Part 2
            int chosenArtist = 0;

            while (true)
            {
                
                request = $"/artist/?name={nameSearch}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                response = await client.GetAsync(request);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ConsoleHelper.PrintColorRed("No available artists yet. Press any key to return to menu and ask your admin to fill the database.");
                    Console.ReadKey();
                    return;
                }
                else if (!response.IsSuccessStatusCode)
                {
                    ConsoleHelper.PrintColorRed($"Failed to get available artists. Status code: {response.StatusCode}");
                }

                content = await response.Content.ReadAsStringAsync();

                artists = JsonSerializer.Deserialize<ListArtistsWithId[]>(content);


                Console.Clear();
                ConsoleHelper.PrintColorGreen("Available artists: ");
                await Console.Out.WriteLineAsync("");
                var table = new ConsoleTable("No", "ARTIST NAME", "DESCRIPTION");
                int i = amountPerPage * (pageNumber - 1) + 1;
                foreach (var artist in artists)
                {
                    table.AddRow(i, artist.Name, artist.Description);
                    i++;
                }
                table.Write(Format.Minimal);

                if (numberOfPages > 1 && pageNumber > 1)
                {
                    ConsoleHelper.PrintColorGreen("<= Press left to see previous page with artists.");
                }

                if (numberOfPages > 1 && pageNumber < numberOfPages)
                {
                    ConsoleHelper.PrintColorGreen("=> Press right to see next page with artists.");
                }

                await Console.Out.WriteLineAsync("Press X to go back to previous menu.");
                await Console.Out.WriteAsync("Choose number to add an artist to your collection: ");


                input = "";

                while (true)// either arrow right-left or 1-2 digits
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (numberOfPages > 1 && pageNumber < numberOfPages && keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        pageNumber++;
                        break;

                    }
                    if (numberOfPages > 1 && pageNumber > 1 && keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        pageNumber--;
                       break;
                    }

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        int amountOnThisPage = pageNumber < numberOfPages ? amountPerPage : amount % amountPerPage;
                        int minArtistNumberThisTable = (pageNumber - 1) * 10 + 1; // 1 on first page but e.g. 11 on second
                        int maxArtistNumberThisTable = amountPerPage * (pageNumber - 1) + amountOnThisPage; // 1 on first page but e.g. 11 on second
                        if (!int.TryParse(input.ToString(), out chosenArtist) || chosenArtist < minArtistNumberThisTable || chosenArtist > maxArtistNumberThisTable)
                        {
                            ConsoleHelper.PrintColorRed("\nInvalid input, try again.");
                            Console.ReadKey();
                        }

                        break;
                    }

                    if (keyInfo.Key == ConsoleKey.X)
                    {
                        return;
                    }

                    input += keyInfo.KeyChar;

                }
                if (chosenArtist != 0) break;

            }

            // Part 3
            // Gets id of the chosen artist in the list and artist's name 
            int chosenArtistInList = chosenArtist - amountPerPage * (pageNumber - 1);
            int artistId = artists
                .Skip(chosenArtistInList - 1)
                .Select(a => a.Id)
                .FirstOrDefault();
            string artistName = artists
                .Skip(chosenArtistInList - 1)
                .Select(a => a.Name)
                .FirstOrDefault();

            ConnectUserToArtist userToArtist = new ConnectUserToArtist()
            {
                Username = username,
                ArtistId = artistId,
            };
            string json = JsonSerializer.Serialize(userToArtist);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            response = await client.PostAsync($"/user/{username}/artist/{artistId}/", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                ConsoleHelper.PrintColorRed($"Failed to connect user to artist (status code {response.StatusCode})");
            }
            else
            {
                ConsoleHelper.PrintColorGreen($"Congratulations, you have added {artistName} to your collection! ");
                // or it was already in user's collection - no control for that now
                Console.ReadKey();
            }
        }



        public static async Task ConnectGenreToUser(HttpClient client, string username)
        {
            // Amandas förslag

            // 1. Method returns available genres in database with genreId
            // 2. User chooses genre to connect with via genreId
            // 3. Connection occurs or exception

            HttpResponseMessage response = await client.GetAsync($"/genre/");

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await Console.Out.WriteLineAsync("No available genre. Press any key to return to menu and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get genres. Status code: {response.StatusCode}");
            }
            string content = await response.Content.ReadAsStringAsync();

            ListGenresWithId[] genres = JsonSerializer.Deserialize<ListGenresWithId[]>(content);
            int amount = genres.Count();

            Console.WriteLine("Available genres:");
            for (int i = 0; i < amount; i++)
            {
                Console.WriteLine($"{i}. {genres[i].Title}");
            }

            Console.WriteLine("Choose a GenreId:");
            int selectedGenreId;

            if (!int.TryParse(Console.ReadLine(), out selectedGenreId) || selectedGenreId < 0 || selectedGenreId >= amount)
            {
                Console.WriteLine("Invalid genre number.");
                return;
            }

            // Connecting user's choosen id with genre 
            int genreId = genres[selectedGenreId].Id;

            ConnectUserToGenre connectUserToGenre = new ConnectUserToGenre()
            {
                Username = username,
                GenreId = genreId
            };

            string json = JsonSerializer.Serialize(connectUserToGenre);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage connectResponse = await client.PostAsync($"/user/{username}/genre/{genreId}/", jsonContent);

            if (connectResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully connected {username} to genre.");
            }
            else
            {
                Console.WriteLine($"Failed to connect {username} to genre.");
            }
        }
    }
}
