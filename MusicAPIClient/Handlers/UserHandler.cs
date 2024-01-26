using MusicAPIClient.APIModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MusicAPIClient.Handlers
{
    // Magda
    public class UserHandler
    {
        public static async Task GetUser(HttpClient client, string name)
        {
            HttpResponseMessage response = await client.GetAsync("/user/{id}");

            Console.Write("Enter id to view specific user");
            string input = Console.ReadLine();

            int id;

            if(int.TryParse(input, out id))
            {
                try
                {
                    var response = await client.GetAsync($"/blog/{id}");
                }

                catch
                {

                }
            }
        }
    }
}
