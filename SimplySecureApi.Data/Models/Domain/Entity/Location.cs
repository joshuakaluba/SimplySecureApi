using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Location : Auditable
    {
        [Display(Name = "Location Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display(Name = "Armed")]
        [JsonProperty("armed")]
        public bool Armed { get; set; }

        [Display(Name = "Silent Alarm")]
        [JsonProperty("isSilentAlarm")]
        public bool IsSilentAlarm { get; set; }

        [Display(Name = "Triggered")]
        [JsonProperty("triggered")]
        [ScaffoldColumn(false)]
        public bool Triggered { get; set; }

        public bool IsNotTriggered => !Triggered;

        [JsonProperty("active")]
        public bool Active { get; set; } = true;
    }
}