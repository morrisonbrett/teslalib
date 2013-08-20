using Newtonsoft.Json;
using System;

namespace TeslaLib.Converters
{
    class UnixTimestampConverter: JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(DateTime));
        }

        /// <summary>
        /// Convert Unix Timestamp to a DateTime object
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var unixTimestamp = serializer.Deserialize<long>(reader);

            // Convert the Unix Timestamp to a readable DateTime
            var time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            time = time.AddSeconds(unixTimestamp).ToLocalTime();

            return time;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
