using Newtonsoft.Json;

namespace TeslaLib.Models
{
    public class ResultStatus
    {
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        [JsonProperty(PropertyName = "result")]
        public bool Result { get; set; }
    }
}
