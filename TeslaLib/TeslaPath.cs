namespace TeslaLib
{
    public static class TeslaPath
    {
        public static string TESLA_STREAMING_SERVER => "https://streaming.vn.teslamotors.com/";
        public static string TESLA_SERVER(bool IsDebugMode) => IsDebugMode ? "https://private-857c-timdorr.apiary.io/" : "https://portal.vn.teslamotors.com/";
        public static string LOGIN_PATH => "login";
        public static string VEHICLES_PATH => "vehicles";
        public static string MOBILE_ENABLED_PATH => "vehicles/{0}/mobile_enabled";
        public static string CHARGE_STATE_PATH => "vehicles/{0}/command/charge_state";
        public static string CLIMATE_STATE_PATH => "vehicles/{0}/command/climate_state";
        public static string DRIVE_STATE_PATH => "vehicles/{0}/command/drive_state";
        public static string GUI_SETTINGS_PATH => "vehicles/{0}/command/gui_settings";
        public static string VEHICLE_STATE_PATH => "vehicles/{0}/command/vehicle_state";
        public static string WAKE_UP_PATH => "vehicles/{0}/command/wake_up";
        public static string CHARGE_PORT_DOOR_OPEN_PATH => "vehicles/{0}/command/charge_port_door_open";
        public static string SET_CHARGE_LIMIT => "vehicles/{0}/command/set_charge_limit?percent={1}";
        public static string CHARGE_START_PATH => "vehicles/{0}/command/charge_start";
        public static string CHARGE_STOP_PATH => "vehicles/{0}/command/charge_stop";
        public static string FLASH_LIGHTS_PATH => "vehicles/{0}/command/flash_lights";
        public static string HONK_HORN_PATH => "vehicles/{0}/command/honk_horn";
        public static string DOOR_UNLOCK_PATH => "vehicles/{0}/command/door_unlock";
        public static string DOOR_LOCK_PATH => "vehicles/{0}/command/door_lock";
        public static string SET_TEMPERATURE_PATH => "vehicles/{0}/command/set_temps?driver_temp={1}&passenger_temp={2}";
        public static string HVAC_START_PATH => "vehicles/{0}/command/auto_conditioning_start";
        public static string HVAC_STOP_PATH => "vehicles/{0}/command/auto_conditioning_stop";
        public static string SUN_ROOF_CONTROL_PATH_WITH_PERCENT => "vehicles/{0}/command/sun_roof_control?state={1}&percent={2}";
        public static string SUN_ROOF_CONTROL_PATH => "vehicles/{0}/command/sun_roof_control?state={1}";
    }
}
