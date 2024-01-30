﻿using ConsoleTables;
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
            //string content = await response.Content.ReadAsStringAsync();
            //ListArtistsWithId[] artists = JsonSerializer.Deserialize<ListArtistsWithId[]>(content);
            //int amount = artists.Count();

            // Part 2

            int pageNumber = 1;
            int amountPerPage = 10;
            int amount = 0;
            int numberOfPages = 0;
            int amountOnThisPage = 0;
            int chosenArtist = 0;
            string request = "";
            string content = "";
            string input = "";
            ListArtistsWithId[] artists;
            await Console.Out.WriteLineAsync("Here you can:");
            await Console.Out.WriteLineAsync("1. Press 1 to see all available artists sorted alphabetically");
            await Console.Out.WriteLineAsync("2. Type first letters of artist name to search for the artist");
            await Console.Out.WriteLineAsync("Press X to go back to previous menu");

            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    input = "";
                    request = $"/artist/?name={input}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                    //request = $"/artist/?pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                    break;

                case "x":
                case "X":
                    return;

                default:
                    request = $"/artist/?name={input}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                    break;
            }


            // This is done in two steps just to show we can use string queries with pagination
            // Otherwise we would show 10 at a time from the same result list
            response = await client.GetAsync($"/artist/?name={input}");
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

            amount = artists.Count();
            numberOfPages = (int)Math.Ceiling((decimal)amount / amountPerPage);
            amountOnThisPage = pageNumber < numberOfPages ? amountPerPage : amount % amountPerPage;
            // above I have prepared numbers that need to be send as parameters in a string query

            while (true)
            {

                if (pageNumber == 1)
                {

                }

                request = $"/artist/?name={input}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                response = await client.GetAsync(request);
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

                int i = amountPerPage * (pageNumber - 1) + 1;
                foreach (var artist in artists)
                {
                    table.AddRow(i, artist.Name, artist.Description);
                    i++;
                }

                Console.Clear();
                await Console.Out.WriteLineAsync("Available artists: ");
                await Console.Out.WriteLineAsync("");
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
                await Console.Out.WriteLineAsync("Choose number to add an artist to your collection.");

                bool enterPressed = false;
                while (!enterPressed)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (numberOfPages > 1 && pageNumber < numberOfPages && keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        pageNumber++;
                        request = $"/artist/?name={input}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                        continue;

                    }
                    if (numberOfPages > 1 && pageNumber > 1 && keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        pageNumber--;
                        request = $"/artist/?name={input}&pageNumber={pageNumber}&amountPerPage={amountPerPage}";
                        continue;
                    }

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        enterPressed = true;
                    }
                    input += keyInfo.KeyChar;
                }


                int minArtistNumberThisTable = (pageNumber - 1) * 10 + 1; // 1 on first page but e.g. 11 on second
                int maxArtistNumberThisTable = amountPerPage * (pageNumber - 1) + amountOnThisPage; // 1 on first page but e.g. 11 on second
                if (!int.TryParse(input.ToString(), out chosenArtist) || chosenArtist < minArtistNumberThisTable || chosenArtist > maxArtistNumberThisTable)
                {
                    await Console.Out.WriteLineAsync("Invalid input.");
                }
                else break;
            }
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
