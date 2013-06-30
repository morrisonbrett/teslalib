using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeslaLib;
using TeslaLib.Models;

namespace FieldAnalyzer
{
    public class FieldAnalyzer
    {

        public FieldAnalyzer()
        {

        }

        public async Task MainAsync()
        {
            await Task.Factory.StartNew(() => Start());
        }

        public async Task Start()
        {
            TeslaClient client = new TeslaClient(true);

            await client.LogIn("username", "password");

            if (client.IsLoggedIn)
            {
                List<TeslaVehicle> cars = await client.LoadVehicles();

                TeslaVehicle car = cars.FirstOrDefault();

                bool showUnchanged = true;

                WriteModifiedFields(await client.AnalyzeFields<TeslaVehicle>(client.TESLA_SERVER, client.VEHICLES_PATH), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFields<ChargeStateStatus>(client.TESLA_SERVER, string.Format(client.CHARGE_STATE_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFields<ClimateStateStatus>(client.TESLA_SERVER, string.Format(client.CLIMATE_STATE_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFields<DriveStateStatus>(client.TESLA_SERVER, string.Format(client.DRIVE_STATE_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFields<GuiSettingsStatus>(client.TESLA_SERVER, string.Format(client.GUI_SETTINGS_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFields<MobileEnabledStatus>(client.TESLA_SERVER, string.Format(client.MOBILE_ENABLED_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFields<VehicleStateStatus>(client.TESLA_SERVER, string.Format(client.VEHICLE_STATE_PATH, car.Id)), showUnchanged);
            }

        }

        private void WriteModifiedFields(Dictionary<string, FieldType> fieldDict, bool showUnchanged = true)
        {
            foreach (string key in fieldDict.Keys)
            {
                FieldType type = fieldDict[key];

                if (showUnchanged || type != FieldType.UNCHANGED)
                {
                    Console.WriteLine(key + " - " + fieldDict[key].ToString());
                }
            }

            Console.WriteLine();
        }
    }
}
