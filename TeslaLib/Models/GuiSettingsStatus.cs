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
    public class GuiSettingsStatus
    {

        public GuiSettingsStatus()
        {

        }

        [JsonProperty(PropertyName = "gui_distance_units")]
        public string DistanceUnits { get; set; }

        [JsonProperty(PropertyName = "gui_temperature_units")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TemperatureUnits TemperatureUnits { get; set; }

        [JsonProperty(PropertyName = "gui_charge_rate_units")]
        public string ChargeRateUnits { get; set; }

        [JsonProperty(PropertyName = "gui_24_hour_time")]
        public bool Is24HourTime { get; set; }

        [JsonProperty(PropertyName = "gui_range_display")]
        public string RangeDisplay { get; set; }

    }

    public enum TemperatureUnits
    {
         [EnumMember(Value = "F")]
        FAHRENHEIT,

        [EnumMember(Value = "C")]
        CELSIUS,
    }
}
