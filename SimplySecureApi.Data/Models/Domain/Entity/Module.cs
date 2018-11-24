using System;
using Newtonsoft.Json;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Module : Auditable
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("state")]
        public bool State { get; set; }

        [JsonProperty("isMotionDetector")]
        public bool IsMotionDetector { get; set; }

        [JsonProperty("locationId")]
        public Guid LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}