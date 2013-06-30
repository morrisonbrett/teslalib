using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TeslaLib.Converters;

namespace TeslaLib.Models
{

    public class TeslaVehicle
    {

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "option_codes")]
        [JsonConverter(typeof (VehicleOptionsConverter))]
        public VehicleOptions Options { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "vehicle_id")]
        public int VehicleId { get; set; }

        [JsonProperty(PropertyName = "vin")]
        public string VIN { get; set; }

        [JsonProperty(PropertyName = "tokens")]
        public List<string> Tokens { get; set; }

        [JsonProperty(PropertyName = "state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public VehicleState State { get; set; }

    }

    public enum VehicleState
    {
        [EnumMember(Value = "Online")]
        ONLINE,

        [EnumMember(Value = "Asleep")]
        ASLEEP,
    }
}