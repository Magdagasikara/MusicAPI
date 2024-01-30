using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MusicAPIClient.Helpers
{
    public static class MenuHelper
    {

        public static async Task HeaderLogin()
        {
            ConsoleHelper.PrintColorGreen("MUSIC API");
            ConsoleHelper.PrintColorGreen("Like Spotify - just without music. And most other functions.");
            await Console.Out.WriteLineAsync("");
        }

        public static async Task HeaderUserAdmin(string username)
        {
            ConsoleHelper.PrintColorGreen($"Welcome to Music API, {username}!");
            ConsoleHelper.PrintColorGreen("---------------------------------");
            await Console.Out.WriteLineAsync("Please enter:");
        }
    }
}
