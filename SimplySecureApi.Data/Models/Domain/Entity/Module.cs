using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Module : Auditable
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public bool State { get; set; }

        [JsonProperty("armed")]
        public bool Armed { get; set; }

        [JsonProperty("isMotionDetector")]
        public bool IsMotionDetector { get; set; }

        [JsonProperty("isSilentAlarm")]
        public bool IsSilentAlarm { get; set; }

        [JsonProperty("triggered")]
        [ScaffoldColumn(false)]
        public bool Triggered { get; set; }
    }
}