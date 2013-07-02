using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeslaLib;
using TeslaLib.Models;

namespace TeslaConsole
{
    public class TeslaConsole
    {
        public async Task MainAsync()
        {
            await Task.Factory.StartNew(() => Start());
        }

        public async Task Start()
        {
            TeslaClient client = new TeslaClient(true);

            await client.LogInAsync("username", "password");

            Console.WriteLine("Logged In: " + client.IsLoggedIn.ToString());
            Console.WriteLine();

            if (client.IsLoggedIn)
            {
                List<TeslaVehicle> cars = await client.LoadVehiclesAsync();

                if (cars.Count == 0)
                {
                    Console.WriteLine("Error: You do not have access to any vehicles");
                    return;
                }

                Console.WriteLine("Vehicles:");
                foreach (TeslaVehicle car in cars)
                {
                    Console.WriteLine(car.Id + " " + car.VIN);
                }

                //TeslaVehicle car = cars.FirstOrDefault();
            }
        }
    }
}
