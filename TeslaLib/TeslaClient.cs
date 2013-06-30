using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeslaLib.Models;

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
                if (IsDebugMode)
                {
                    return "https://private-857c-timdorr.apiary.io/";
                }
                else
                {
                    return "https://portal.vn.teslamotors.com/";
                }
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

        public string CHARGE_MODE_PATH
        {
            get
            {
                return "vehicles/{0}/command/charge_standard";
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

        private TeslaWebClient webClient = new TeslaWebClient();

        public TeslaClient(bool isDebugMode = false)
        {
            IsDebugMode = isDebugMode;
            IsLoggedIn = false;
        }

        public async Task TestCommands()
        {
            string username = "test@test.com";
            string password = "password";

            bool success = await LogIn(username, password);

            List<TeslaVehicle> vehicles = await LoadVehicles();
            
            TeslaVehicle car = vehicles.FirstOrDefault();

            if (car != null)
            {
                var b = await LoadChargeStateStatus(car);
                var c = await LoadClimateStateStatus(car);
                var d = await LoadDriveStateStatus(car);
                var e = await LoadGuiStateStatus(car);
                var f = await LoadMobileEnabledStatus(car);
                var g = await LoadVehicleStateStatus(car);

                var h = await FlashLights(car);
                var i = await HonkHorn(car);
                var j = await LockDoors(car);
                var k = await OpenChargePortDoor(car);
                var l = await SetChargeMode(car);
                var m = await SetPanoramicRoofLevel(car, PanoramicRoofState.COMFORT);
                var n = await SetTemperatureSettings(car, 17, 17);
                var o = await StartCharge(car);
                var p = await StartHVAC(car);
                var q = await StopCharge(car);
                var r = await StopHVAC(car);
                var s = await UnlockDoors(car);
                
                await WakeUp(car);

                Console.WriteLine("Executed All Commands");
            }
        }

        private string RemoveComments(string str)
        {
            return Regex.Replace(str, @"//(.*?)\r?\n", "\n");
        }

        public async Task<Dictionary<string, FieldType>> AnalyzeFields<T>(string server, string path)
        {
            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(server, path));

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
            Dictionary<string, FieldType> fieldDict = new Dictionary<string, FieldType>();

            try
            {
                response = RemoveComments(response);

                JObject rootObject = null;

                JToken token = JToken.Parse(response);
                if (token.Type == JTokenType.Array)
                {
                    rootObject = token.First as JObject;
                }
                else if (token.Type == JTokenType.Object)
                {
                    rootObject = token as JObject;
                }


                List<string> currentFields = rootObject.Properties().Select(p => p.Name).ToList();

                List<string> expectedFields = typeof(T).GetProperties()
                    .Where(p => p.IsDefined(typeof(JsonPropertyAttribute), false))
                    .Select(p => ((JsonPropertyAttribute[])p.GetCustomAttributes(typeof(JsonPropertyAttribute), false)).Single().PropertyName).ToList();

                List<string> newFields = currentFields.Except(expectedFields).ToList();
                List<string> removedFields = expectedFields.Except(currentFields).ToList();
                List<string> sameFields = currentFields.Except(newFields).ToList();

                foreach (string field in newFields)
                {
                    fieldDict.Add(field, FieldType.ADDED);
                }

                foreach (string field in removedFields)
                {
                    fieldDict.Add(field, FieldType.REMOVED);
                }

                foreach (string field in sameFields)
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

        public async Task<bool> LogIn(string username, string password)
        {
            try
            {

                // Set up POST parameters.
                var values = new NameValueCollection() 
                 { 
                    { "user_session[email]", username },
                    { "user_session[password]", password }
                 };

                // Perform the POST request.
                byte[] data = await webClient.UploadValuesTaskAsync(Path.Combine(TESLA_SERVER, LOGIN_PATH), values);
                
                string response = System.Text.Encoding.ASCII.GetString(data);

                if (response.Contains("You do not have access"))
                {
                    IsLoggedIn = false;
                }
                else
                {
                    IsLoggedIn = true;
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

        public async Task<List<TeslaVehicle>> LoadVehicles()
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, VEHICLES_PATH));

                List<TeslaVehicle> vehicles = ParseVehicles(response);

                return vehicles;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<MobileEnabledStatus> LoadMobileEnabledStatus(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(MOBILE_ENABLED_PATH, vehicle.Id)));

                MobileEnabledStatus status = ParseMobileEnabledStatus(response);

                return status;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ChargeStateStatus> LoadChargeStateStatus(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                ChargeStateStatus status = ParseChargeStateStatus(response);

                return status;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ClimateStateStatus> LoadClimateStateStatus(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CLIMATE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                ClimateStateStatus status = ParseClimateStateStatus(response);

                return status;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<DriveStateStatus> LoadDriveStateStatus(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(DRIVE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                DriveStateStatus status = ParseDriveStateStatus(response);

                return status;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<GuiSettingsStatus> LoadGuiStateStatus(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(GUI_SETTINGS_PATH, vehicle.Id)));

                GuiSettingsStatus status = ParseGuiStateStatus(response);

                return status;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<VehicleStateStatus> LoadVehicleStateStatus(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(VEHICLE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                VehicleStateStatus status = ParseVehicleStateStatus(response);

                return status;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task WakeUp(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(WAKE_UP_PATH, vehicle.Id)));
            }
            catch (Exception e)
            {

            }
        }

        public async Task<ResultStatus> OpenChargePortDoor(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_PORT_DOOR_OPEN_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> SetChargeMode(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {

                // TODO: Check for v4.5
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_MODE_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StartCharge(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STATE_PATH, vehicle.Id)));

                response = RemoveComments(response);

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StopCharge(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(CHARGE_STOP_PATH, vehicle.Id)));

                response = RemoveComments(response);

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> FlashLights(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(FLASH_LIGHTS_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> HonkHorn(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(HONK_HORN_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> UnlockDoors(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(DOOR_UNLOCK_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> LockDoors(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(DOOR_LOCK_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> SetTemperatureSettings(TeslaVehicle vehicle, int driverTemp = 17, int passengerTemp = 17)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                int TEMP_MAX = 32;
                int TEMP_MIN = 17;

                driverTemp = Math.Max(driverTemp, TEMP_MIN);
                driverTemp = Math.Min(driverTemp, TEMP_MAX);

                passengerTemp = Math.Max(passengerTemp, TEMP_MIN);
                passengerTemp = Math.Min(passengerTemp, TEMP_MAX);

                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(SET_TEMPERATURE_PATH, vehicle.Id, driverTemp, passengerTemp)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StartHVAC(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(HVAC_START_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> StopHVAC(TeslaVehicle vehicle)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(HVAC_STOP_PATH, vehicle.Id)));

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public async Task<ResultStatus> SetPanoramicRoofLevel(TeslaVehicle vehicle, PanoramicRoofState roofState, int percentOpen = 0)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                if (vehicle.Options.RoofType != RoofType.NONE)
                {
                    return new ResultStatus() { Result = false, Reason = "No Panoramic Roof" };
                }

                string response = "";

                if (roofState == PanoramicRoofState.MOVE)
                {
                    response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(SUN_ROOF_CONTROL_PATH_WITH_PERCENT, vehicle.Id, roofState.GetEnumValue(), percentOpen)));
                }
                else
                {
                    response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format(SUN_ROOF_CONTROL_PATH, vehicle.Id, roofState.GetEnumValue())));
                }

                ResultStatus result = ParseResultStatus(response);

                return result;
            }
            catch (Exception e)
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
        public async Task<string> LoadStreamingValues(TeslaVehicle vehicle, string values)
        {
            if (!IsLoggedIn)
            {
                throw new AuthenticationException("Error: Not logged in");
            }

            if (vehicle == null)
            {
                throw new ArgumentNullException("vehicle");
            }

            try
            {
                values = "speed,odometer,soc,elevation,est_heading,est_lat,est_lng,power,shift_state,range,est_range";

                string response = await webClient.DownloadStringTaskAsync(Path.Combine(TESLA_SERVER, string.Format("stream/{0}/?values={1}", vehicle.VehicleId, values)));

                //ResultStatus result = ParseResultStatus(response);

                return response;
                //return result;
            }
            catch (Exception e)
            {

            }

            return null;
        }

        #endregion

        #region Response Parsers

        public List<TeslaVehicle> ParseVehicles(string response)
        {
            List<TeslaVehicle> vehicles = JsonConvert.DeserializeObject<List<TeslaVehicle>>(response);

            TeslaVehicle first = vehicles.FirstOrDefault();

            return vehicles;
        }

        public MobileEnabledStatus ParseMobileEnabledStatus(string response)
        {
            MobileEnabledStatus status = JsonConvert.DeserializeObject<MobileEnabledStatus>(response);

            return status;
        }

        public ChargeStateStatus ParseChargeStateStatus(string response)
        {
            ChargeStateStatus status = JsonConvert.DeserializeObject<ChargeStateStatus>(response);

            return status;
        }

        public ClimateStateStatus ParseClimateStateStatus(string response)
        {
            ClimateStateStatus status = JsonConvert.DeserializeObject<ClimateStateStatus>(response);

            return status;
        }

        public DriveStateStatus ParseDriveStateStatus(string response)
        {
            DriveStateStatus status = JsonConvert.DeserializeObject<DriveStateStatus>(response);

            return status;
        }

        public GuiSettingsStatus ParseGuiStateStatus(string response)
        {
            GuiSettingsStatus status = JsonConvert.DeserializeObject<GuiSettingsStatus>(response);

            return status;
        }

        public VehicleStateStatus ParseVehicleStateStatus(string response)
        {
            VehicleStateStatus status = JsonConvert.DeserializeObject<VehicleStateStatus>(response);

            return status;
        }

        public ResultStatus ParseResultStatus(string response)
        {
            ResultStatus status = JsonConvert.DeserializeObject<ResultStatus>(response);

            return status;
        }

        #endregion

        public void Dispose()
        {
            if (webClient != null)
            {
                webClient.Dispose();
            }
        }
    }
}