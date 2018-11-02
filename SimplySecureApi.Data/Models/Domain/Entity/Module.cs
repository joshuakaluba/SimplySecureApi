using Newtonsoft.Json;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Module : Auditable
    {
        [JsonProperty("state")]
        public bool State { get; set; }
    }
}