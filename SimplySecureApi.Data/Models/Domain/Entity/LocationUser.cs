using Newtonsoft.Json;
using SimplySecureApi.Data.Models.Authentication;
using System;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class LocationUser : Auditable
    {
        public Guid LocationId { get; set; }

        public string ApplicationUserId { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin
        {
            get
            {
                if (Location != null)
                {
                    return Location.ApplicationUserId == ApplicationUserId;
                }

                return false;
            }
        }

        [JsonProperty("email")]
        public string Email => ApplicationUser != null ? ApplicationUser.Email : "";

        [JsonProperty("name")]
        public string Name => ApplicationUser != null ? ApplicationUser.FullName : "";

        public virtual Location Location { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}