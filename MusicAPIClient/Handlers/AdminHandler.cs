using Microsoft.AspNetCore.Http;
using MusicAPI.Models;
using MusicAPIClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicAPIClient.Handlers
{
    // Amanda
    public class AdminHandler
    {
        // Method that collects all users in array using ApiModel 'ListUsers'
        public static async Task GetAllUsers(HttpClient client)
        {
            HttpResponseMessage response = await client.GetAsync("/user");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to list users. Status code: {response.StatusCode}");
            }

            string content = await response.Content.ReadAsStringAsync();

            ListUsers[] listUsers = JsonSerializer.Deserialize<ListUsers[]>(content);

            foreach (var user in listUsers)
            {
                await Console.Out.WriteLineAsync($"Username:{user.Name}");
            }

            await Console.Out.WriteLineAsync("Press enter to return to menu");
            Console.ReadLine();
            Console.Clear();
        }

        // Using Api Model 'AddUser' to create new user
        public static async Task AddUser(HttpClient client)
        {
            Console.Clear();
            Console.Write("Enter username: ");
            string name = Console.ReadLine();

            AddUser addUser = new AddUser()
            {
                Name = name,
            };

            string json = JsonSerializer.Serialize(addUser);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/user/", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync("User added successfully.");
            }

            else
            {
                Console.WriteLine($"Failed to add user. Status code: {response.StatusCode}");
            }

            await Console.Out.WriteLineAsync("Press enter to return to menu");
            Console.ReadLine();
            Console.Clear();
        }

        // Same procedure but using 'AddGenre' ApiModel to add new genre
        public static async Task AddGenre(HttpClient client)
        {
            Console.Clear();
            Console.Write("Enter name of genre: ");
            string title = Console.ReadLine();

            AddGenre addGenre = new AddGenre()
            {
                Title = title,
            };

            string json = JsonSerializer.Serialize(addGenre);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/genre/", jsonContent);


            if (response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync("Genre added successfully.");
            }

            else
            {
                Console.WriteLine($"Failed to add genre. Status code: {response.StatusCode}");
            }

            await Console.Out.WriteLineAsync("Press enter to go back to menu");
            Console.ReadLine();
            Console.Clear();
        }

        // Api Model 'AddSong'
        public static async Task AddSong(HttpClient client)
        {
            Console.Clear();
            Console.Write("Enter name of song: ");
            string name = Console.ReadLine();

            Console.Write("Enter name of artist: ");
            string artist = Console.ReadLine();

            Console.Write("Enter name of genre: ");
            string genre = Console.ReadLine();

            AddSong addSong = new AddSong()
            {
                Name = name,
                Artist = artist,
                Genre = genre
            };

            string json = JsonSerializer.Serialize(addSong);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/song/", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync("song added successfully.");
            }

            else
            {
                Console.WriteLine($"Failed to add song. Status code: {response.StatusCode}");
            }

            await Console.Out.WriteLineAsync("Press enter to go back to menu");
            Console.ReadLine();
            Console.Clear();
        }

        // Api Model 'AddArtist'
        public static async Task AddArtist(HttpClient client)
        {
            Console.Clear();
            Console.Write("Enter name of artist: ");
            string name = Console.ReadLine();

            Console.Write("Enter description of artist: ");
            string description = Console.ReadLine();

            Console.Write("Enter genre of artist: ");
            string genre = Console.ReadLine();

            AddArtist addArtist = new AddArtist()
            {
                Name = name,
                Description = description,
                Genre = genre
            };

            string json = JsonSerializer.Serialize(addArtist);

            StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/artist/", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                await Console.Out.WriteLineAsync("Artist added successfully.");
            }

            else
            {
                Console.WriteLine($"Failed to add artist. Status code: {response.StatusCode}");
            }

            await Console.Out.WriteLineAsync("Press enter to go back to main menu");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
