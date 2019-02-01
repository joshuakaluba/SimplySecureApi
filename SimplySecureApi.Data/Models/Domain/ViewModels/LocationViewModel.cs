using Newtonsoft.Json;
using System;

namespace SimplySecureApi.Data.Models.Domain.ViewModels
{
    public class LocationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("armed")]
        public bool Armed { get; set; }

        [JsonProperty("isSilentAlarm")]
        public bool IsSilentAlarm { get; set; }

        [JsonProperty("triggered")]
        public bool Triggered { get; set; }
    }
}