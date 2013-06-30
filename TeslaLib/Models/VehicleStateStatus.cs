using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TeslaLib.Models
{
    public class VehicleStateStatus
    {

        public VehicleStateStatus()
        {

        }

        [JsonProperty(PropertyName = "df")]
        public bool IsDriverFrontDoorOpen { get; set; }

        [JsonProperty(PropertyName = "dr")]
        public bool IsDriverRearDoorOpen { get; set; }

        [JsonProperty(PropertyName = "pf")]
        public bool IsPassengerFrontDoorOpen { get; set; }

        [JsonProperty(PropertyName = "pr")]
        public bool IsPassengerRearDoorOpen { get; set; }

        [JsonProperty(PropertyName = "ft")]
        public bool IsFrontTrunkOpen { get; set; }

        [JsonProperty(PropertyName = "rt")]
        public bool IsRearTrunkOpen { get; set; }

        [JsonProperty(PropertyName = "car_verson")]
        public string CarVersion { get; set; }

        [JsonProperty(PropertyName = "locked")]
        public bool IsLocked { get; set; }

        [JsonProperty(PropertyName = "sun_roof_installed")]
        public bool HasPanoramicRoof { get; set; }

        [JsonProperty(PropertyName = "sun_roof_state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PanoramicRoofState PanoramicRoofState { get; set; }

        [JsonProperty(PropertyName = "sun_roof_percent_open")]
        public int PanoramicRoofPercentOpen { get; set; }

        [JsonProperty(PropertyName = "dark_rims")]
        public bool HasDarkRims { get; set; }

        [JsonProperty(PropertyName = "wheel_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WheelType WheelType { get; set; }

        [JsonProperty(PropertyName = "has_spoiler")]
        public bool HasSpoiler { get; set; }

        [JsonProperty(PropertyName = "roof_color")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RoofType RoofColor { get; set; }

        [JsonProperty(PropertyName = "perf_config")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PerformanceConfiguration PerformanceConfiguration { get; set; }
    }

    public enum PanoramicRoofState
    {
        [EnumMember(Value = "Open")]
        OPEN,

        [EnumMember(Value = "Comfort")]
        COMFORT,

        [EnumMember(Value = "Vent")]
        VENT,

        [EnumMember(Value = "Close")]
        CLOSE,

        [EnumMember(Value = "MOve")]
        MOVE,

        [EnumMember(Value = "Unknown")]
        UNKNOWN,
    }

    public enum WheelType
    {

        [EnumMember(Value = "Base19")]
        BASE_19,

        [EnumMember(Value = "Silver21")]
        SILVER_21,

        [EnumMember(Value = "Charcoal21")]
        CHARCOAL_21,

        CHARCOAL_PERFORMANCE_21
    }

    public enum RoofType
    {
        [EnumMember(Value ="Colored")]
        COLORED,

        [EnumMember(Value = "None")]
        NONE,

        [EnumMember(Value = "Black")]
        BLACK
    }

    public enum PerformanceConfiguration
    {
        [EnumMember(Value = "Base")]
        BASE,

        [EnumMember(Value = "Sport")]
        SPORT

    }
}