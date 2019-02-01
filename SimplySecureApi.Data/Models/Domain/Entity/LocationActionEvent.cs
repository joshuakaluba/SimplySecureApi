using Newtonsoft.Json;
using SimplySecureApi.Data.Models.Authentication;
using System;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class LocationActionEvent : Auditable
    {
        public LocationActionEnum Action { get; set; }

        [JsonProperty("actionEvent")]
        public string ActionEvent => Enum.GetName(typeof(LocationActionEnum), Action);

        [JsonProperty("locationId")]
        public Guid LocationId { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual Location Location { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [JsonProperty("caller")]
        public string Caller => ApplicationUser != null ? ApplicationUser.Email : "Automated";
    }
}