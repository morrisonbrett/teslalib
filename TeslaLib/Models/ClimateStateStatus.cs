using Newtonsoft.Json;

namespace TeslaLib.Models
{
    public class ClimateStateStatus
    {
        /// <summary>
        /// Degrees C inside the car
        /// </summary>
        [JsonProperty(PropertyName = "inside_temp")]
        public double InsideTemperature { get; set; }

        /// <summary>
        /// Degrees C outside of the car
        /// </summary>
        [JsonProperty(PropertyName = "outside_temp")]
        public double? OutsideTemperature { get; set; }

        /// <summary>
        /// Degress C of the driver temperature setpoint
        /// </summary>
        [JsonProperty(PropertyName = "driver_temp_setting")]
        public double DriverTemperatureSetting { get; set; }

        /// <summary>
        /// Degress C of the passenger temperature setpoint
        /// </summary>
        [JsonProperty(PropertyName = "passenger_temp_setting")]
        public double PassengerTemperatureSetting { get; set; }

        [JsonProperty(PropertyName = "is_auto_conditioning_on")]
        public bool? IsAutoAirConditioning { get; set; }

        [JsonProperty(PropertyName = "is_front_defroster_on")]
        public bool? IsFrontDefrosterOn { get; set; }

        [JsonProperty(PropertyName = "is_rear_defroster_on")]
        public bool? IsRearDefrosterOn { get; set; }

        /// <summary>
        /// Fan Speed
        /// 0-6 or null
        /// </summary>
        [JsonProperty(PropertyName = "fan_status")]
        public int? FanStatus { get; set; }
    }
}