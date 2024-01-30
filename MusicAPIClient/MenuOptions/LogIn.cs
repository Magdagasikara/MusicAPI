﻿using Microsoft.EntityFrameworkCore.Query.Internal;
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

            await Console.Out.WriteAsync("Enter your username: ");
            username = Console.ReadLine();

            if (username.ToUpper() == "X") { return; }

            HttpResponseMessage response = await client.GetAsync($"/user/{username}");

            while (!response.IsSuccessStatusCode)
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
                    }
                    // --------
                    // lyfta ut det till UserHandler som AddUser
                    // -----OBS
                }
                else
                {
                    ConsoleHelper.PrintColorRed("You have nothing to do here without logging in. Bye bye!");
                    Console.ReadKey();
                    return;
                }

                response = await client.GetAsync($"/user/{username}");
            }

            // if admin: go to AdminMenu
            if (username.ToUpper() == "ADMIN")
            {
                await AdminMenu.AdminMenuOptions(client, username);
            }

            // if user: go to UserMenu 
            else
            {
                response = await client.GetAsync($"/user/{username}");

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
                Console.WriteLine("No user is currently logged in.");
            }
        }
    }
}