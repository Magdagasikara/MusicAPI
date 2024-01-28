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

        public static Task GetSongsForUser(HttpClient client, string username)
        {
            throw new NotImplementedException();
        }
    }
}
