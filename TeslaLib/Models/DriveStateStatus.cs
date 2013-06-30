using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TeslaLib.Converters;

namespace TeslaLib.Models
{
    public class DriveStateStatus
    {
        public DriveStateStatus()
        {

        }

        [JsonProperty(PropertyName = "shift_state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ShiftState? ShiftState { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public int? Speed { get; set; }

        /// <summary>
        /// Degrees N of the equator
        /// </summary>
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// Degrees W of the prime meridian
        /// </summary>
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// Integer compass heading (0-359)
        /// </summary>
        [JsonProperty(PropertyName = "heading")]
        public int Heading { get; set; }

        [JsonProperty(PropertyName = "gps_as_of")]
        [JsonConverter(typeof(UnixTimestampConverter))]
        public DateTime GpsAsOf { get; set; }

    }

    public enum ShiftState
    {
        [EnumMember(Value = "D")]
        DRIVE,

        [EnumMember(Value = "N")]
        NEUTRAL,
        
        [EnumMember(Value = "P")]
        PARK,

        [EnumMember(Value = "R")]
        REVERSE,        
    }
}