using System;
using System.Linq;
using System.Threading.Tasks;
using TeslaLib;

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
            var client = new TeslaClient(true);

            await client.LogInAsync("username", "password");

            Console.WriteLine("Logged In: " + client.IsLoggedIn);
            Console.WriteLine();

            if (client.IsLoggedIn)
            {
                var cars = await client.LoadVehiclesAsync();

                if (cars.Count == 0)
                {
                    Console.WriteLine("Error: You do not have access to any vehicles");
                    return;
                }

                Console.WriteLine("Vehicles:");
                foreach (var c in cars)
                {
                    Console.WriteLine(c.Id + " " + c.VIN);
                }

                var car = cars.FirstOrDefault();
                if (car == null) return;

                var a = client.LoadChargeStateStatus(car);
                Console.WriteLine("Charging State: " + a.ChargingState);
                Console.WriteLine("Battery Range: " + a.BatteryRange);
            }
        }
    }
}
