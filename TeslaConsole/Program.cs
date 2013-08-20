using System;

namespace TeslaConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var console = new TeslaConsole();

            console.MainAsync().Wait();

            Console.ReadKey();
        }
    }
}
