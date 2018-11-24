using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Location : Auditable
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("armed")]
        public bool Armed { get; set; }

        [JsonProperty("isSilentAlarm")]
        public bool IsSilentAlarm { get; set; }

        [JsonProperty("triggered")]
        [ScaffoldColumn(false)]
        public bool Triggered { get; set; }
    }
}