using Microsoft.EntityFrameworkCore.Query.Internal;
using MusicAPIClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MusicAPI.Data;
using Microsoft.EntityFrameworkCore;
using MusicAPIClient.Helpers;
using MusicAPI.Handlers;
using MusicAPI.Services;
using MusicAPIClient.MenuOptions;
using MusicAPI.Repositories;

namespace MusicAPIClient.MenuOptions
{
    public static class LogIn
    {
        private static string username;
        public static async Task LogInUser(HttpClient client)
        {
            Console.Clear();

            Console.SetCursorPosition(0, 5);
            await Console.Out.WriteLineAsync("Press X to exit");

            Console.SetCursorPosition(0, 0);
            MenuHelper.HeaderLogin();

            while (true)
            {
                await Console.Out.WriteAsync("Enter your username: ");
                username = Console.ReadLine();

                if (username.ToUpper() == "X") Environment.Exit(0);
                else if (username == "") continue;

                HttpResponseMessage response = await client.GetAsync($"/user/{username}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.Clear();
                    MenuHelper.HeaderLogin();

                    await Console.Out.WriteAsync($"User {username} doesn't exist. Do you wish to create a new account? (Y/N) ");
                    string input = Console.ReadLine();

                    if (input.ToUpper() == "Y" || input.ToUpper() == "YES" || input.ToUpper() == "J" || input.ToUpper() == "JA")
                    {
                        // OBS-----
                        // lyfta ut det till UserHandler som AddUser
                        // --------
                        AddUser user = new AddUser()
                        {
                            Name = username
                        };
                        string json = JsonSerializer.Serialize(user);

                        StringContent jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                        response = await client.PostAsync("/user/", jsonContent);
                        if (!response.IsSuccessStatusCode)
                        {
                            ConsoleHelper.PrintColorRed($"Failed to create user (status code {response.StatusCode})");
                            Console.ReadKey();
                        }
                        // --------
                        // lyfta ut det till UserHandler som AddUser
                        // -----OBS
                        response = await client.GetAsync($"/user/{username}");
                        break;
                    }
                    else
                    {
                        ConsoleHelper.PrintColorRed("\nTry to log in with another username or press X to exit.");
                        Console.ReadKey();
                        continue;
                    }
                }
                else { break; }
                
            }

            // if admin: go to AdminMenu
            if (username.ToUpper() == "ADMIN")
            {
                await AdminMenu.AdminMenuOptions(client, username);
            }

            // if user: go to UserMenu 
            else
            {
                HttpResponseMessage response = await client.GetAsync($"/user/{username}");

                string content = await response.Content.ReadAsStringAsync();

                ListUsers user = JsonSerializer.Deserialize<ListUsers>(content);

                await UserMenu.UserMenuOptions(client, user.Name);
            }
        }

        public static void LogOutUser()
        {
            if (!string.IsNullOrEmpty(username))
            {
                Console.WriteLine($"User {username} has been successfully logged out");
                username = null;
            }
            else
            {
                ConsoleHelper.PrintColorRed("No user is currently logged in.");
            }
        }
    }
}