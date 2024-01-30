using ConsoleTables;
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
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MusicAPI.Repositories;
using MusicAPI.Models.Dtos;

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

            var table = new ConsoleTable("USERNAME");

            foreach (var user in listUsers)
            {
                table.AddRow(user.Name);
            }
            table.Write(Format.Minimal);
            Console.ReadKey();

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
        public static async Task Add50SongsFromArtist(HttpClient client, ISpotifyHelper spotifyHelper)
        {
            await spotifyHelper.SaveArtistGenreAndTrackFromSpotifyToDb(searchQuery);
        }
        public static async Task AddTop100ArtistsTop10Songs(HttpClient client, ISpotifyHelper spotifyHelper)
        {
        }
    }
}
