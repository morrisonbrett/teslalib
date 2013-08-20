using System;

namespace FieldAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var analyzer = new FieldAnalyzer();

            analyzer.MainAsync().Wait();

            Console.ReadKey();
        }
    }
}
