using ConsoleTables;
using MusicAPI.Models;
using MusicAPIClient.APIModels;
using MusicAPIClient.Helpers;
using System;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MusicAPIClient.Handlers
{
   
    public class UserHandler
    {
        public static async Task GetArtistsForUser(HttpClient client, string username)
        {

            // OBS lägg till omredigering från BadRequest
            // "vill du lägga till artister nu?" och skicka till den metoden

            HttpResponseMessage response = await client.GetAsync($"/user/{username}/artist/");
            Console.Clear();

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

                ListArtists[] artists = JsonSerializer.Deserialize<ListArtists[]>(content);

                ConsoleHelper.PrintColorGreen("Artists in your collection: ");
                ConsoleHelper.PrintColorGreen("(it must be soo satisfying to read their names over and over again - wow!)");
                await Console.Out.WriteLineAsync("");

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
            Console.Clear();

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

                ConsoleHelper.PrintColorGreen("Songs in your collection: ");
                ConsoleHelper.PrintColorGreen("(why would you listen to them when you can read their names?!)");
                await Console.Out.WriteLineAsync("");

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
            Console.Clear();

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

                ConsoleHelper.PrintColorGreen("Genres in your collection: ");
                await Console.Out.WriteLineAsync("");

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

            // Part 1 checks if there are any songs to connect to (but no control to exclude the ones in user's current collection!)
            // Part 2 shows filtered och alphabetical list, user can choose an song to connect to
            // Part 3 connect the song to user


            int pageNumber = 1;
            int amountPerPage = 20;


            // Part 1

            string input = "";
            string request = "";
            string songSearch = "";

            while (true)
            {
                Console.Clear();
                ConsoleHelper.PrintColorGreen("To see available songs choose: ");
                ConsoleHelper.PrintColorGreen("--------------------------------");
                await Console.Out.WriteLineAsync("1. To see all songs in alphabetical order");
                await Console.Out.WriteLineAsync("2. To search for an song title");
                await Console.Out.WriteLineAsync("X. To go back to previous menu");

                input = Console.ReadLine();
                
                switch (input)
                {
                    case "1":
                        request = $"/song/";
                        break;

                    case "2":
                        await Console.Out.WriteAsync("Type text you want to search for: ");
                        songSearch = Console.ReadLine();
                        request = $"/song/?name={songSearch}";
                        break;

                    case "x":
                    case "X":
                        return;

                    default:
                        ConsoleHelper.PrintColorRed("Invalid input, try again.");
                        Console.ReadLine();
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
                await Console.Out.WriteLineAsync("No available songs. Press any key and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                ConsoleHelper.PrintColorRed($"Failed to get songs. Status code: {response.StatusCode}");
            }
            string content = await response.Content.ReadAsStringAsync();
            ListSongs[] songs = JsonSerializer.Deserialize<ListSongs[]>(content);
            int amount = songs.Count();
            int numberOfPages = (int)Math.Ceiling((decimal)amount / amountPerPage);
            if (amount == 0)
            {
                await Console.Out.WriteLineAsync("No available songs. Press a key and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }


            // Part 2
            int chosenSong = 0;

            while (true)
            {

                request = $"/song/?name={songSearch}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                response = await client.GetAsync(request);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ConsoleHelper.PrintColorRed("No available songs yet. Press any key and ask your admin to fill the database.");
                    Console.ReadKey();
                    return;
                }
                else if (!response.IsSuccessStatusCode)
                {
                    ConsoleHelper.PrintColorRed($"Failed to get available artists. Status code: {response.StatusCode}");
                }

                content = await response.Content.ReadAsStringAsync();

                songs = JsonSerializer.Deserialize<ListSongs[]>(content);


                Console.Clear();
                ConsoleHelper.PrintColorGreen($"There are {amount} songs you can choose among: ");
                await Console.Out.WriteLineAsync("");

                var table = new ConsoleTable("No", "SONG", "ARTIST", "GENRE");
                int i = amountPerPage * (pageNumber - 1) + 1;
                foreach (var song in songs)
                {
                    table.AddRow(i, song.Name, song.Artist, song.Genre);
                    i++;
                }
                table.Write(Format.Minimal);

                if (numberOfPages > 1 && pageNumber > 1)
                {
                    ConsoleHelper.PrintColorGreen("<= Press left to see previous page with songs.");
                }

                if (numberOfPages > 1 && pageNumber < numberOfPages)
                {
                    ConsoleHelper.PrintColorGreen("=> Press right to see next page with songs.");
                }

                await Console.Out.WriteLineAsync("Press X to go back to previous menu.");
                await Console.Out.WriteAsync("Choose number to add an song to your collection: ");


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
                        int minSongNumberThisTable = (pageNumber - 1) * 10 + 1; // 1 on first page but e.g. 11 on second
                        int maxSongNumberThisTable = amountPerPage * (pageNumber - 1) + amountOnThisPage; // 1 on first page but e.g. 11 on second
                        if (!int.TryParse(input.ToString(), out chosenSong) || chosenSong < minSongNumberThisTable || chosenSong > maxSongNumberThisTable)
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
                if (chosenSong != 0) break;

            }

            // Part 3
            // Gets id of the chosen artist in the list and artist's name 
            int chosenSongInList = chosenSong - amountPerPage * (pageNumber - 1);
            int songId = songs
                .Skip(chosenSongInList - 1)
                .Select(a => a.Id)
                .FirstOrDefault();
            string songName = songs
                .Skip(chosenSongInList - 1)
                .Select(a => a.Name)
                .FirstOrDefault();

            ConnectUserToSong userToSong = new ConnectUserToSong()
            {
                Username = username,
                SongId = songId,
            };
            string json = JsonSerializer.Serialize(userToSong);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            response = await client.PostAsync($"/user/{username}/song/{songId}/", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                ConsoleHelper.PrintColorRed($"Failed to connect user to song (status code {response.StatusCode})");
                Console.ReadKey();
            }
            else
            {
                ConsoleHelper.PrintColorGreen($"Congratulations, you have added {songName} to your collection! ");
                // or it was already in user's collection - no control for that now
                Console.ReadKey();
            }
        }

        public static async Task ConnectArtistToUser(HttpClient client, string username)
        {
            // Part 1 checks if there are any artists to connect to (but no control to exclude the ones in user's current collection!)
            // Part 2 shows filtered och alphabetical list, user can choose an artist to connect to
            // Part 3 connect the artist to user

            int pageNumber = 1;
            int amountPerPage = 10;


            // Part 1

           
            string input = "";
            string request = "";
            string nameSearch = "";

            while (true)
            {
                Console.Clear();
                ConsoleHelper.PrintColorGreen("To see available artists choose: ");
                ConsoleHelper.PrintColorGreen("--------------------------------");
                await Console.Out.WriteLineAsync("1. To see all artists in alphabetical order");
                await Console.Out.WriteLineAsync("2. To search for an artist name");
                await Console.Out.WriteLineAsync("X. To go back to previous menu");
                
                input = Console.ReadLine();

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
                        Console.ReadLine();
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
            ListArtists[] artists = JsonSerializer.Deserialize<ListArtists[]>(content);
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

                artists = JsonSerializer.Deserialize<ListArtists[]>(content);


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
                Console.ReadKey();
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
            // Part 1 checks if there are any genres to connect to (but no control to exclude the ones in user's current collection!)
            // Part 2 shows filtered och alphabetical list, user can choose an genres to connect to
            // Part 3 connect the genres to user

            int pageNumber = 1;
            int amountPerPage = 10;

            // Part 1

            string input = "";
            string request = "";
            string genreSearch = "";

            while (true)
            {
                Console.Clear();
                ConsoleHelper.PrintColorGreen("To see available genres choose: ");
                ConsoleHelper.PrintColorGreen("--------------------------------");
                await Console.Out.WriteLineAsync("1. To see all genres in alphabetical order");
                await Console.Out.WriteLineAsync("2. To search for a specific genre");
                await Console.Out.WriteLineAsync("X. To go back to previous menu");

                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        request = $"/genre/";
                        break;

                    case "2":
                        await Console.Out.WriteAsync("Type text you want to search for: ");
                        genreSearch = Console.ReadLine();
                        request = $"/genre/?name={genreSearch}";
                        break;

                    case "x":
                    case "X":
                        return;

                    default:
                        ConsoleHelper.PrintColorRed("Invalid input, try again.");
                        Console.ReadLine();
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
                await Console.Out.WriteLineAsync("No available genres. Press any key to return to menu and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }
            else if (!response.IsSuccessStatusCode)
            {
                ConsoleHelper.PrintColorRed($"Failed to get genres. Status code: {response.StatusCode}");
            }
            string content = await response.Content.ReadAsStringAsync();
            ListGenres[] genres = JsonSerializer.Deserialize<ListGenres[]>(content);
            int amount = genres.Count();
            int numberOfPages = (int)Math.Ceiling((decimal)amount / amountPerPage);
            if (amount == 0)
            {
                await Console.Out.WriteLineAsync("No available genres. Press a key and ask your admin to fill the database.");
                Console.ReadKey();
                return;
            }


            // Part 2
            int chosenGenre = 0;

            while (true)
            {

                request = $"/genre/?title={genreSearch}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                response = await client.GetAsync(request);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ConsoleHelper.PrintColorRed("No available genres yet. Press any key to return to menu and ask your admin to fill the database.");
                    Console.ReadKey();
                    return;
                }
                else if (!response.IsSuccessStatusCode)
                {
                    ConsoleHelper.PrintColorRed($"Failed to get available genres. Status code: {response.StatusCode}");
                }

                content = await response.Content.ReadAsStringAsync();

                genres = JsonSerializer.Deserialize<ListGenres[]>(content);


                Console.Clear();
                ConsoleHelper.PrintColorGreen("Available genres: ");
                await Console.Out.WriteLineAsync("");

                var table = new ConsoleTable("No", "GENRE");
                int i = amountPerPage * (pageNumber - 1) + 1;
                foreach (var genre in genres)
                {
                    table.AddRow(i, genre.Title);
                    i++;
                }
                table.Write(Format.Minimal);

                if (numberOfPages > 1 && pageNumber > 1)
                {
                    ConsoleHelper.PrintColorGreen("<= Press left to see previous page with genres.");
                }

                if (numberOfPages > 1 && pageNumber < numberOfPages)
                {
                    ConsoleHelper.PrintColorGreen("=> Press right to see next page with genres.");
                }

                await Console.Out.WriteLineAsync("Press X to go back to previous menu.");
                await Console.Out.WriteAsync("Choose number to add an genre to your collection: ");


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
                        int minGenreNumberThisTable = (pageNumber - 1) * 10 + 1; // 1 on first page but e.g. 11 on second
                        int maxGenreNumberThisTable = amountPerPage * (pageNumber - 1) + amountOnThisPage; // 1 on first page but e.g. 11 on second
                        if (!int.TryParse(input.ToString(), out chosenGenre) || chosenGenre < minGenreNumberThisTable || chosenGenre > maxGenreNumberThisTable)
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
                if (chosenGenre != 0) break;

            }

            // Part 3
            // Gets id of the chosen genre in the list and genre's name 
            int chosenGenreInList = chosenGenre - amountPerPage * (pageNumber - 1);
            int genreId = genres
                .Skip(chosenGenreInList - 1)
                .Select(a => a.Id)
                .FirstOrDefault();
            string genreTitle = genres
                .Skip(chosenGenreInList - 1)
                .Select(a => a.Title)
                .FirstOrDefault();

            ConnectUserToGenre userToGenre = new ConnectUserToGenre()
            {
                Username = username,
                GenreId = genreId,
            };
            string json = JsonSerializer.Serialize(userToGenre);
            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            response = await client.PostAsync($"/user/{username}/genre/{genreId}/", jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                ConsoleHelper.PrintColorRed($"Failed to connect user to genre (status code {response.StatusCode})");
                Console.ReadKey();
            }
            else
            {
                ConsoleHelper.PrintColorGreen($"Congratulations, you have added {genreTitle} to your collection! ");
                // or it was already in user's collection - no control for that now
                Console.ReadKey();
            }
        }
    }
}
