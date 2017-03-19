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
        public async Task MainAsync()
        {
            await Task.Factory.StartNew(() => Start());
        }

        public async Task Start()
        {
            var client = new TeslaClient(true);

            await client.LogInAsync("username", "password");

            if (client.IsLoggedIn)
            {
                var cars = await client.LoadVehiclesAsync();

                var car = cars.FirstOrDefault();

                const bool showUnchanged = true;
                WriteModifiedFields(await client.AnalyzeFieldsAsync<TeslaVehicle>(TeslaPath.TESLA_SERVER(client.IsDebugMode), TeslaPath.VEHICLES_PATH), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFieldsAsync<ChargeStateStatus>(TeslaPath.TESLA_SERVER(client.IsDebugMode), string.Format(TeslaPath.CHARGE_STATE_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFieldsAsync<ClimateStateStatus>(TeslaPath.TESLA_SERVER(client.IsDebugMode), string.Format(TeslaPath.CLIMATE_STATE_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFieldsAsync<DriveStateStatus>(TeslaPath.TESLA_SERVER(client.IsDebugMode), string.Format(TeslaPath.DRIVE_STATE_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFieldsAsync<GuiSettingsStatus>(TeslaPath.TESLA_SERVER(client.IsDebugMode), string.Format(TeslaPath.GUI_SETTINGS_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFieldsAsync<MobileEnabledStatus>(TeslaPath.TESLA_SERVER(client.IsDebugMode), string.Format(TeslaPath.MOBILE_ENABLED_PATH, car.Id)), showUnchanged);
                WriteModifiedFields(await client.AnalyzeFieldsAsync<VehicleStateStatus>(TeslaPath.TESLA_SERVER(client.IsDebugMode), string.Format(TeslaPath.VEHICLE_STATE_PATH, car.Id)), showUnchanged);
            }

        }

        private void WriteModifiedFields(Dictionary<string, FieldType> fieldDict, bool showUnchanged = true)
        {
            foreach (var key in fieldDict.Keys)
            {
                var type = fieldDict[key];

                if (showUnchanged || type != FieldType.UNCHANGED)
                {
                    Console.WriteLine(key + " - " + fieldDict[key].ToString());
                }
            }

            Console.WriteLine();
        }
    }
}
