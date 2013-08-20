using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeslaLib.Models;
using TeslaLib.Streaming;

// ReSharper disable InconsistentNaming
// ReSharper disable EmptyGeneralCatchClause
// ReSharper disable PossibleNullReferenceException
namespace TeslaLib
{
    public class TeslaClient : IDisposable
    {
        #region API Paths

        public string TESLA_STREAMING_SERVER
        {
            get
            {
                return "https://streaming.vn.teslamotors.com/";
            }
        }

        public string TESLA_SERVER
        {
            get
            {
                return IsDebugMode ? "https://private-857c-timdorr.apiary.io/" : "https://portal.vn.teslamotors.com/";
            }
        }

        public string LOGIN_PATH
        {
            get
            {
                return "login";
            }
        }

        public string VEHICLES_PATH
        {
            get
            {
                return "vehicles";
            }
        }

        public string MOBILE_ENABLED_PATH
        {
            get
            {
                return "vehicles/{0}/mobile_enabled";
            }
        }

        public string CHARGE_STATE_PATH
        {
            get
            {
                return "vehicles/{0}/command/charge_state";
            }
        }

        public string CLIMATE_STATE_PATH
        {
            get
            {
                return "vehicles/{0}/command/climate_state";
            }
        }

        public string DRIVE_STATE_PATH
        {
            get
            {
                return "vehicles/{0}/command/drive_state";
            }
        }

        public string GUI_SETTINGS_PATH
        {
            get
            {
                return "vehicles/{0}/command/gui_settings";
            }
        }

        public string VEHICLE_STATE_PATH
        {
            get
            {
                return "vehicles/{0}/command/vehicle_state";
            }
        }

        public string WAKE_UP_PATH
        {
            get
            {
                return "vehicles/{0}/command/wake_up";
            }
        }

        public string CHARGE_PORT_DOOR_OPEN_PATH
        {
            get
            {
                return "vehicles/{0}/command/charge_port_door_open";
            }
        }

        public string SET_CHARGE_LIMIT
        {
            get
            {
                return "vehicles/{0}/command/set_charge_limit?percent={1}";
            }
        }

        public string CHARGE_START_PATH
        {
            get
            {
                return "vehicles/{0}/command/charge_start";
            }
        }

        public string CHARGE_STOP_PATH
        {
            get
            {
                return "vehicles/{0}/command/charge_stop";
            }
        }

        public string FLASH_LIGHTS_PATH
        {
            get
            {
                return "vehicles/{0}/command/flash_lights";
            }
        }

        public string HONK_HORN_PATH
        {
            get
            {
                return "vehicles/{0}/command/honk_horn";
            }
        }

        public string DOOR_UNLOCK_PATH
        {
            get
            {
                return "vehicles/{0}/command/door_unlock";
            }
        }

        public string DOOR_LOCK_PATH
        {
            get
            {
                return "vehicles/{0}/command/door_lock";
            }
        }

        public string SET_TEMPERATURE_PATH
        {
            get
            {
                return "vehicles/{0}/command/set_temps?driver_temp={1}&passenger_temp={2}";
            }
        }

        public string HVAC_START_PATH
        {
            get
            {
                return "vehicles/{0}/command/auto_conditioning_start";
            }
        }

        public string HVAC_STOP_PATH
        {
            get
            {
                return "vehicles/{0}/command/auto_conditioning_stop";
            }
        }

        public string SUN_ROOF_CONTROL_PATH_WITH_PERCENT
        {
            get
            {
                return "vehicles/{0}/command/sun_roof_control?state={1}&percent={2}";
            }
        }

        public string SUN_ROOF_CONTROL_PATH
        {
            get
            {
                return "vehicles/{0}/command/sun_roof_control?state={1}";
            }
        }

        #endregion

        public bool IsDebugMode { get; private set; }

        public bool IsLoggedIn { get; private set; }

        public string Email { get; private set; }

        private readonly TeslaWebClient webClient = new TeslaWebClient();

        public TeslaClient(bool isDebugMode = false)
        {
            IsDebugMode = isDebugMode;
            IsLoggedIn = false;
        }

        public void TestCommands()
        {
            // ReSharper disable UnusedVariable
            const string username = "test@test.com";
            const string password = "password";

            var success = LogIn(username, password);

            var vehicles = LoadVehicles();

            var car = vehicles.FirstOrDefault();

            if (car != null)
            {
                var b = LoadChargeStateStatus(car);
                var c = LoadClimateStateStatus(car);
                var d = LoadDriveStateStatus(car);
                var e = LoadGuiStateStatus(car);
                var f = LoadMobileEnabledStatus(car);
                var g = LoadVehicleStateStatus(car);

                var h = FlashLights(car);
                var i = HonkHorn(car);
                var j = LockDoors(car);
                var k = OpenChargePortDoor(car);
                var l = SetChargeLimit(car, 80);
                var m = SetPanoramicRoofLevel(car, PanoramicRoofState.COMFORT);
                var n = SetTemperatureSettings(car, 17, 17);
                var o = StartCharge(car);
                var p = StartHVAC(car);
                var q = StopCharge(car);
                var r = StopHVAC(car);
                var s = UnlockDoors(car);

                WakeUp(car);

                Console.WriteLine("Executed All Commands");
            }
            // ReSharper restore UnusedVariable
        }

        private static string RemoveComments(string str)
        {
            return Regex.Replace(str, @"//(.*?)\r?\n", "\n");
        }

        public async Task<Dictionary<string, FieldType>> AnalyzeFieldsAsync<T>(string server, string path)
        {
            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(server, path));

                return AnalyzeFields<T>(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Cannot get response from web client");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                return null;
            }
        }

        public Dictionary<string, FieldType> AnalyzeFields<T>(string server, string path)
        {
            try
            {
                var response = webClient.DownloadString(Path.Combine(server, path));

                return AnalyzeFields<T>(response);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Cannot get response from web client");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);

                return null;
            }
        }

        public Dictionary<string, FieldType> AnalyzeFields<T>(string response)
        {
            var fieldDict = new Dictionary<string, FieldType>();

            try
            {
                response = RemoveComments(response);

                JObject rootObject = null;

                var token = JToken.Parse(response);
                if (token.Type == JTokenType.Array)
                {
                    rootObject = token.First as JObject;
                }
                else if (token.Type == JTokenType.Object)
                {
                    rootObject = token as JObject;
                }


                var currentFields = rootObject.Properties().Select(p => p.Name).ToList();

                var expectedFields = typeof(T).GetProperties()
                    .Where(p => p.IsDefined(typeof(JsonPropertyAttribute), false))
                    .Select(p => ((JsonPropertyAttribute[])p.GetCustomAttributes(typeof(JsonPropertyAttribute), false)).Single().PropertyName).ToList();

                var newFields = currentFields.Except(expectedFields).ToList();
                var removedFields = expectedFields.Except(currentFields).ToList();
                var sameFields = currentFields.Except(newFields).ToList();

                foreach (var field in newFields)
                {
                    fieldDict.Add(field, FieldType.ADDED);
                }

                foreach (var field in removedFields)
                {
                    fieldDict.Add(field, FieldType.REMOVED);
                }

                foreach (var field in sameFields)
                {
                    fieldDict.Add(field, FieldType.UNCHANGED);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Cannot analyze fields");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return fieldDict;
        }

        #region Web Methods

        private void RequireLogin()
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }
        }

        private static void RequireVehicle(TeslaVehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }
        }

        public bool LogIn(string username, string password)
        {
            try
            {
                // Set up POST parameters.
                var values = new NameValueCollection
                { 
                    { "user_session[email]", username },
                    { "user_session[password]", password }
                 };

                // Perform the POST request.
                var data = webClient.UploadValues(Path.Combine(TESLA_SERVER, LOGIN_PATH), values);

                var response = Encoding.ASCII.GetString(data);

                if (response.Contains("You do not have access"))
                {
                    IsLoggedIn = false;
                }
                else
                {
                    IsLoggedIn = true;
                    Email = username;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Cannot Login to the Tesla Remote API");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<TeslaVehicle> LoadVehicles()
        {
            RequireLogin();

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, VEHICLES_PATH));

                var vehicles = ParseVehicles(response);

                return vehicles;
            }

            catch (Exception)

            {

            }

            return null;
        }

        public MobileEnabledStatus LoadMobileEnabledStatus(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(MOBILE_ENABLED_PATH, vehicle.Id)));

                var status = ParseMobileEnabledStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ChargeStateStatus LoadChargeStateStatus(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseChargeStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ClimateStateStatus LoadClimateStateStatus(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(CLIMATE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseClimateStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public DriveStateStatus LoadDriveStateStatus(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(DRIVE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseDriveStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public GuiSettingsStatus LoadGuiStateStatus(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(GUI_SETTINGS_PATH, vehicle.Id)));

                var status = ParseGuiStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public VehicleStateStatus LoadVehicleStateStatus(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(VEHICLE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseVehicleStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public void WakeUp(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(WAKE_UP_PATH, vehicle.Id)));
            }
            catch (Exception)
            {

            }
        }

        public ResultStatus OpenChargePortDoor(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(CHARGE_PORT_DOOR_OPEN_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus SetChargeLimit(TeslaVehicle vehicle, int percent = 50)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            percent = Math.Max(percent, 50);
            percent = Math.Min(percent, 100);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(SET_CHARGE_LIMIT, vehicle.Id, percent)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus StartCharge(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus StopCharge(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STOP_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus FlashLights(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(FLASH_LIGHTS_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus HonkHorn(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(HONK_HORN_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus UnlockDoors(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(DOOR_UNLOCK_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus LockDoors(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(DOOR_LOCK_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus SetTemperatureSettings(TeslaVehicle vehicle, int driverTemp = 17, int passengerTemp = 17)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                const int TEMP_MAX = 32;
                const int TEMP_MIN = 17;

                driverTemp = Math.Max(driverTemp, TEMP_MIN);
                driverTemp = Math.Min(driverTemp, TEMP_MAX);

                passengerTemp = Math.Max(passengerTemp, TEMP_MIN);
                passengerTemp = Math.Min(passengerTemp, TEMP_MAX);

                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(SET_TEMPERATURE_PATH, vehicle.Id, driverTemp, passengerTemp)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus StartHVAC(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(HVAC_START_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus StopHVAC(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(HVAC_STOP_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public ResultStatus SetPanoramicRoofLevel(TeslaVehicle vehicle, PanoramicRoofState roofState, int percentOpen = 0)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                if (vehicle.Options.RoofType != RoofType.NONE)
                {
                    return new ResultStatus { Result = false, Reason = "No Panoramic Roof" };
                }

                string response;

                if (roofState == PanoramicRoofState.MOVE)
                {
                    response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(SUN_ROOF_CONTROL_PATH_WITH_PERCENT, vehicle.Id, roofState.GetEnumValue(), percentOpen)));
                }
                else
                {
                    response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format(SUN_ROOF_CONTROL_PATH, vehicle.Id, roofState.GetEnumValue())));
                }

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        /// <summary>
        /// WARNING: Have not been able to test this
        /// speed,odometer,soc,elevation,est_heading,est_lat,est_lng,power,shift_state,range,est_range
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public string LoadStreamingValues(TeslaVehicle vehicle, string values)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                values = "speed,odometer,soc,elevation,est_heading,est_lat,est_lng,power,shift_state,range,est_range";

                var response = webClient.DownloadString(Path.Combine(TESLA_SERVER, string.Format("stream/{0}/?values={1}", vehicle.VehicleId, values)));

                return response;
            }
            catch (Exception)
            {

            }

            return null;
        }

        #endregion

        #region Web Methods Async

        public async Task<bool> LogInAsync(string username, string password)
        {
            try
            {
                // Set up POST parameters.
                var values = new NameValueCollection
                             { 
                    { "user_session[email]", username },
                    { "user_session[password]", password }
                 };

                // Perform the POST request.
                var data = await webClient.UploadValuesTaskAsync(Path.Combine(TESLA_SERVER, LOGIN_PATH), values);

                var response = Encoding.ASCII.GetString(data);

                if (response.Contains("You do not have access"))
                {
                    IsLoggedIn = false;
                }
                else
                {
                    IsLoggedIn = true;
                    Email = username;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Cannot Login to the Tesla Remote API");
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<List<TeslaVehicle>> LoadVehiclesAsync()
        {
            RequireLogin();

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, VEHICLES_PATH));

                var vehicles = ParseVehicles(response);

                return vehicles;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<MobileEnabledStatus> LoadMobileEnabledStatusAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(MOBILE_ENABLED_PATH, vehicle.Id)));

                var status = ParseMobileEnabledStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ChargeStateStatus> LoadChargeStateStatusAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseChargeStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ClimateStateStatus> LoadClimateStateStatusAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CLIMATE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseClimateStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<DriveStateStatus> LoadDriveStateStatusAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(DRIVE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseDriveStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<GuiSettingsStatus> LoadGuiStateStatusAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(GUI_SETTINGS_PATH, vehicle.Id)));

                var status = ParseGuiStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<VehicleStateStatus> LoadVehicleStateStatusAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(VEHICLE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var status = ParseVehicleStateStatus(response);

                return status;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task WakeUpAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(WAKE_UP_PATH, vehicle.Id)));
            }
            catch (Exception)
            {

            }
        }

        public async Task<ResultStatus> OpenChargePortDoorAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_PORT_DOOR_OPEN_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> SetChargeLimitAsync(TeslaVehicle vehicle, int percent = 50)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            percent = Math.Max(percent, 50);
            percent = Math.Min(percent, 100);

            try
            {
                // TODO: Check for v4.5
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(SET_CHARGE_LIMIT, vehicle.Id, percent)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StartChargeAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StopChargeAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STOP_PATH, vehicle.Id)));

                response = RemoveComments(response);

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> FlashLightsAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(FLASH_LIGHTS_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> HonkHornAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(HONK_HORN_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> UnlockDoorsAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(DOOR_UNLOCK_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> LockDoorsAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(DOOR_LOCK_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> SetTemperatureSettingsAsync(TeslaVehicle vehicle, int driverTemp = 17, int passengerTemp = 17)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                const int TEMP_MAX = 32;
                const int TEMP_MIN = 17;

                driverTemp = Math.Max(driverTemp, TEMP_MIN);
                driverTemp = Math.Min(driverTemp, TEMP_MAX);

                passengerTemp = Math.Max(passengerTemp, TEMP_MIN);
                passengerTemp = Math.Min(passengerTemp, TEMP_MAX);

                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(SET_TEMPERATURE_PATH, vehicle.Id, driverTemp, passengerTemp)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StartHVACAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(HVAC_START_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StopHVACAsync(TeslaVehicle vehicle)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(HVAC_STOP_PATH, vehicle.Id)));

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public async Task<ResultStatus> SetPanoramicRoofLevelAsync(TeslaVehicle vehicle, PanoramicRoofState roofState, int percentOpen = 0)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                if (vehicle.Options.RoofType != RoofType.NONE)
                {
                    return new ResultStatus { Result = false, Reason = "No Panoramic Roof" };
                }

                string response;

                if (roofState == PanoramicRoofState.MOVE)
                {
                    response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(SUN_ROOF_CONTROL_PATH_WITH_PERCENT, vehicle.Id, roofState.GetEnumValue(), percentOpen)));
                }
                else
                {
                    response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(SUN_ROOF_CONTROL_PATH, vehicle.Id, roofState.GetEnumValue())));
                }

                var result = ParseResultStatus(response);

                return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        /// <summary>
        /// speed,odometer,soc,elevation,est_heading,est_lat,est_lng,power,shift_state,range,est_range
        /// </summary>
        /// <param name="vehicle"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<string> LoadStreamingValuesAsync(TeslaVehicle vehicle, string values)
        {
            RequireLogin();
            RequireVehicle(vehicle);

            try
            {
                values = "speed,odometer,soc,elevation,est_heading,est_lat,est_lng,power,shift_state,range,est_range";

                var response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format("stream/{0}/?values={1}", vehicle.VehicleId, values)));

                //ResultStatus result = ParseResultStatus(response);

                return response;
                //return result;
            }
            catch (Exception)
            {

            }

            return null;
        }

        #endregion

        #region Response Parsers

        public List<TeslaVehicle> ParseVehicles(string response)
        {
            var vehicles = JsonConvert.DeserializeObject<List<TeslaVehicle>>(response);

            var first = vehicles.FirstOrDefault();

            return vehicles;
        }

        public MobileEnabledStatus ParseMobileEnabledStatus(string response)
        {
            var status = JsonConvert.DeserializeObject<MobileEnabledStatus>(response);

            return status;
        }

        public ChargeStateStatus ParseChargeStateStatus(string response)
        {
            var status = JsonConvert.DeserializeObject<ChargeStateStatus>(response);

            return status;
        }

        public ClimateStateStatus ParseClimateStateStatus(string response)
        {
            var status = JsonConvert.DeserializeObject<ClimateStateStatus>(response);

            return status;
        }

        public DriveStateStatus ParseDriveStateStatus(string response)
        {
            var status = JsonConvert.DeserializeObject<DriveStateStatus>(response);

            return status;
        }

        public GuiSettingsStatus ParseGuiStateStatus(string response)
        {
            var status = JsonConvert.DeserializeObject<GuiSettingsStatus>(response);

            return status;
        }

        public VehicleStateStatus ParseVehicleStateStatus(string response)
        {
            var status = JsonConvert.DeserializeObject<VehicleStateStatus>(response);

            return status;
        }

        public ResultStatus ParseResultStatus(string response)
        {
            var status = JsonConvert.DeserializeObject<ResultStatus>(response);

            return status;
        }

        #endregion

        public async void StreamingTest(TeslaVehicle vehicle, string outputDirectory, string tripName, int outputFormatMode,
            string valuesToStream = null)
        {
            if (valuesToStream == null)
            {
                valuesToStream = "speed,odometer,soc,elevation,est_heading,est_lat,est_lng,power,shift_state,range,est_range";
            }

            if (vehicle.Tokens.Count == 0)
            {
                // RELOAD TOKENS
                await WakeUpAsync(vehicle);
                var cars = await LoadVehiclesAsync();

                vehicle = cars.FirstOrDefault(c => c.Id == vehicle.Id);
            }

            var strBasicAuthInfo = string.Format("{0}:{1}", Email, vehicle.Tokens[0]);

            var isStopStreaming = false;

            var outputFormat = (StreamingOutputFormat)Enum.Parse(typeof(StreamingOutputFormat), outputFormatMode.ToString(CultureInfo.InvariantCulture));

            var extension = "txt";

            switch (outputFormat)
            {
                case StreamingOutputFormat.PLAIN_TEXT:
                    extension = "txt";
                    break;
                case StreamingOutputFormat.KML_PLACEMARK:
                case StreamingOutputFormat.KML_PATH:
                    extension = "kml";
                    break;
            }

            // Find the Streamer using Reflection
            var t = GetTypeWithStreamingOutputAttribute(outputFormat);
            var streamer = (AStreamer) Activator.CreateInstance(t);
            
            var filePath = Path.Combine(outputDirectory, string.Format("{0}_{1}.{2}", DateTime.Now.ToString("MM_dd_yyyy"), tripName, extension));

            streamer.Setup(filePath, valuesToStream, tripName);
            streamer.BeforeStreaming();

            while (!isStopStreaming)
            {
                var request = WebRequest.CreateHttp(new Uri(Path.Combine(TESLA_STREAMING_SERVER, "stream", vehicle.VehicleId.ToString(CultureInfo.InvariantCulture), "?values=" + valuesToStream)));
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(strBasicAuthInfo));
                request.Timeout = 12500; // a bit more than the expected 2 minute max long poll

                var response = (HttpWebResponse)await request.GetResponseAsync();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            string line;

                            while ((line = reader.ReadLine()) != null)
                            {
                                // DATA RECEIVED
                                streamer.DataRecevied(line);
                            }
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                    {
                        // RELOAD TOKENS
                        await WakeUpAsync(vehicle);
                        var cars = await LoadVehiclesAsync();

                        vehicle = cars.FirstOrDefault(c => c.Id == vehicle.Id);
                    }
                        break;
                    default:
                        isStopStreaming = true;
                        break;
                }
            }

            streamer.AfterStreaming();
        }

        private static Type GetTypeWithStreamingOutputAttribute(StreamingOutputFormat format)
        {
            var attributeType = typeof(StreamingFormatAttribute);
            
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsDefined(attributeType)) continue;
                var a = type.GetCustomAttribute<StreamingFormatAttribute>();
                    
                if (a.OutputFormat == format)
                {
                    return type;
                }
            }

            return null;
        }

        public void Dispose()
        {
            if (webClient != null)
            {
                webClient.Dispose();
            }
        }
    }
}
// ReSharper restore InconsistentNaming
// ReSharper restore EmptyGeneralCatchClause
// ReSharper restore PossibleNullReferenceException
