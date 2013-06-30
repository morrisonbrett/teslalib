using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaLib;
using TeslaLib.Models;

namespace FieldAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FieldAnalyzer analyzer = new FieldAnalyzer();

            analyzer.MainAsync().Wait();

            Console.ReadKey();
        }

        
    }
}
