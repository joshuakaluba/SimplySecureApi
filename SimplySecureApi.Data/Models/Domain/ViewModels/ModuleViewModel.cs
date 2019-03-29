using Newtonsoft.Json;
using System;

namespace SimplySecureApi.Data.Models.Domain.ViewModels
{
    public class ModuleViewModel : BaseComponentViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("locationId")]
        public Guid LocationId { get; set; } = Guid.NewGuid();

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("isMotionDetector")]
        public bool IsMotionDetector { get; set; }

        [JsonProperty("lastHeartbeat")]
        public DateTime LastHeartbeat { get; set; } = DateTime.MinValue;
    }
}