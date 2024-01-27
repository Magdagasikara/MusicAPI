using Microsoft.EntityFrameworkCore.Query.Internal;
using MusicAPIClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicAPIClient.MenuOptions
{
    public static class LogIn
    {
        public static async Task LogInUser(HttpClient client)
        {
            await Console.Out.WriteAsync("Enter your username: ");
            string username = Console.ReadLine();

            HttpResponseMessage response = await client.GetAsync($"/user/{username}");

            while (!response.IsSuccessStatusCode)
            {
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
                        await Console.Out.WriteLineAsync($"Failed to create user (status code {response.StatusCode})");
                    }
                    // --------
                    // lyfta ut det till UserHandler som AddUser
                    // -----OBS
                }
                else
                {
                    await Console.Out.WriteLineAsync("You have nothing to do here without logging in. Bye bye!");
                    Console.ReadKey();
                    return;
                }

                response = await client.GetAsync($"/user/{username}");

            }

            // if admin: go to AdminMenu
            if (username.ToUpper() == "ADMIN")
            {
                AdminMenu.AdminMenuOptions();
            }

            // if user: go to UserMenu 
            else
            {
                response = await client.GetAsync($"/user/{username}");

                string content = await response.Content.ReadAsStringAsync();

                GetSingleUser user = JsonSerializer.Deserialize<GetSingleUser>(content);

                UserMenu.UserMenuOptions(client, user.Name);
            }

        }
    }
}