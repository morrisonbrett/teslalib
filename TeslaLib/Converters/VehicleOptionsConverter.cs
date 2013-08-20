using Newtonsoft.Json;
using System;
using TeslaLib.Models;

namespace TeslaLib.Converters
{
    class VehicleOptionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(VehicleOptions));
        }

        /// <summary>
        /// Convert the Option Codes into a VehicleOptions instance
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var encoded = serializer.Deserialize<string>(reader);

            var options = new VehicleOptions(encoded);

            return options;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
