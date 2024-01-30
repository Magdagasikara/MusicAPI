using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAPIClient.Helpers
{
    public static class ConsoleHelper
    {

       

        public static async Task PrintColorRed(string input)
        {

            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync(input);
            Console.ForegroundColor = originalColor;
        }


        public static async Task PrintColorGreen(string input)
        {

            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            await Console.Out.WriteLineAsync(input);
            Console.ForegroundColor = originalColor;
        }

    }
}
