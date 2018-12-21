using Newtonsoft.Json;
using System;

namespace SimplySecureApi.Data.Models.Domain.ViewModels
{
    public class LocationViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("armed")]
        public bool Armed { get; set; }

        [JsonProperty("isSilentAlarm")]
        public bool IsSilentAlarm { get; set; } = false;

        [JsonProperty("triggered")]
        public bool Triggered { get; set; }

        public bool Active { get; set; } = true;
    }
}