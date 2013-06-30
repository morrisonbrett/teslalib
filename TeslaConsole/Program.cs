using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaLib;

namespace TeslaConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TeslaConsole console = new TeslaConsole();

            console.MainAsync().Wait();

            Console.ReadKey();
        }
    }
}
