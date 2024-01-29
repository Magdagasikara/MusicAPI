using ConsoleTables;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MusicAPIClient.APIModels;
using System;
using System.Collections.Generic;
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

                ListArtist[] artists = JsonSerializer.Deserialize<ListArtist[]>(content);

                var table = new ConsoleTable("ARTIST NAME", "DESCRIPTION");

                foreach (var artist in artists)
                {
                    table.AddRow(artist.Name, artist.Description);
                }
                table.Write();
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

                ListSongs[] songs = JsonSerializer.Deserialize<ListSongs[]>(content);

                var table = new ConsoleTable("SONG", "ARTIST", "GENRE");

                foreach (var song in songs)
                {
                    table.AddRow(song.Name, song.Artist, song.Genre);
                }
                table.Write();
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

                ListGenres[] genres = JsonSerializer.Deserialize<ListGenres[]>(content);

                var table = new ConsoleTable("GENRE");

                foreach (var genre in genres)
                {
                    table.AddRow(genre.Title);
                }
                table.Write();
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


            throw new NotImplementedException();
        }

        public static async Task ConnectArtistToUser(HttpClient client, string username)
        {
            // Connect Artist To User
            // Part 1 checks if there are any artists to connect
            // Part 2 choose an artist to connect
            // Part 3 connect the artist to user


            // Part 1
            HttpResponseMessage response = await client.GetAsync($"/artist/");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await Console.Out.WriteLineAsync("No available artists. Press any key to return to menu and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to get artists. Status code: {response.StatusCode}");
            }
            string content = await response.Content.ReadAsStringAsync();
            ListArtistsWithId[] artists = JsonSerializer.Deserialize<ListArtistsWithId[]>(content);
            int amount = artists.Count();


            // Part 2

            int pageNumber = 1;
            int amountPerPage = 10;
            int numberOfPages = (int)Math.Ceiling((decimal)amount / amountPerPage);
            int chosenArtist = 0;

            while (true)
            {
                Console.Clear();
                await Console.Out.WriteLineAsync("Here you can:");
                await Console.Out.WriteLineAsync("1. Press 1 to see all available artists sorted alphabetically");
                await Console.Out.WriteLineAsync("2. Type first letters of artist name to search for the artist");
                await Console.Out.WriteLineAsync("3. Press X to go back to previous menu");

                string input = Console.ReadLine();

                int amountOnThisPage = pageNumber < numberOfPages ? amountPerPage : amount % amountPerPage;

                switch (input)
                {
                    case "1":
                        response = await client.GetAsync($"/artist/?pageNumber={pageNumber}&amountPerPage={amountPerPage}");
                        break;

                    case "x":
                    case "X":
                        return;

                    default:
                        response = await client.GetAsync($"/artist/?name={input}&pageNumber={pageNumber}&amountPerPage={amountPerPage}");
                        break;
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await Console.Out.WriteLineAsync("No available artists yet. Press any key to return to menu and ask your admin to fill the database.");
                    Console.ReadKey();
                    return;
                }
                else if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to get available artists. Status code: {response.StatusCode}");
                }

                content = await response.Content.ReadAsStringAsync();

                artists = JsonSerializer.Deserialize<ListArtistsWithId[]>(content);

                var table = new ConsoleTable("No", "ARTIST NAME", "DESCRIPTION");

                int i = 1;
                foreach (var artist in artists)
                {
                    table.AddRow(i, artist.Name, artist.Description);
                    i++;
                }
                table.Write();

                await Console.Out.WriteLineAsync("Choose number to add an artist to your collection.");
                await Console.Out.WriteLineAsync("Press X to go back to previous menu.");
                if (numberOfPages > 1 && pageNumber < numberOfPages)
                {
                    await Console.Out.WriteLineAsync("Press right to see next page with artists.");
                }
                else if (numberOfPages > 1 && pageNumber > 1)
                {
                    await Console.Out.WriteLineAsync("Press left to see previous page with artists.");
                }
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                if (numberOfPages > 1 && pageNumber < numberOfPages && keyInfo.Key == ConsoleKey.RightArrow)
                {
                    pageNumber++;
                    continue;

                }
                if (numberOfPages > 1 && pageNumber > 1 && keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    pageNumber--;
                    continue;
                }

                input = keyInfo.KeyChar + Console.ReadLine();
                if (!int.TryParse(input.ToString(), out chosenArtist) || chosenArtist < 1 || chosenArtist > amountOnThisPage)
                {
                    await Console.Out.WriteLineAsync("Invalid input.");
                }
                else break;
            }
            // Gets id of the chosen artist in the list and artist's name 
            int artistId = artists
                .Skip(chosenArtist - 1)
                .Select(a => a.Id)
                .FirstOrDefault();
            string artistName = artists
                .Skip(chosenArtist - 1)
                .Select(a => a.Name)
                .FirstOrDefault();

            // Part 3 connect the artist to user
            // seems totally unnecessary to create and post json but OK
            // we send both username and artistId directly in the endpoint...
            // but PostAsync needs a second parameter...
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
                await Console.Out.WriteLineAsync($"Failed to connect user to artist (status code {response.StatusCode})");
            }
            else
            {
                await Console.Out.WriteLineAsync($"Congratulations, you have added {artistName} to your collection! ");
            }
        }
    }
}
