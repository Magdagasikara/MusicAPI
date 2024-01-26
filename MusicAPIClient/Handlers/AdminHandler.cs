using MusicAPIClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicAPIClient.Handlers
{
    // Amanda
    public class AdminHandler
    {
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
        }
    }
}
