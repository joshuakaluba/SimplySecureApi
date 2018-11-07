using Newtonsoft.Json;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Module : Auditable
    {
        [JsonProperty("name")]
        public bool Name { get; set; }

        [JsonProperty("state")]
        public bool State { get; set; }

        [JsonProperty("armed")]
        public bool Armed { get; set; }

        [JsonProperty("triggered")]
        public bool Triggered { get; set; }
    }
}