using ConsoleTables;
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
            else {

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
                await Console.Out.WriteLineAsync($"{response.StatusCode}");
                Console.ReadKey();
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
                await Console.Out.WriteLineAsync($"{response.StatusCode}");
                Console.ReadKey();
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
    }
}
