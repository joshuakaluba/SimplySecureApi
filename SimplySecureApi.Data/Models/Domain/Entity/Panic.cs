using Newtonsoft.Json;
using SimplySecureApi.Data.Models.Authentication;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Panic : Auditable
    {
        [JsonProperty("email")]
        public string Email => ApplicationUser != null ? ApplicationUser.Email : "";

        [JsonProperty("name")]
        public string Name => ApplicationUser != null ? ApplicationUser.FullName : "";

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}