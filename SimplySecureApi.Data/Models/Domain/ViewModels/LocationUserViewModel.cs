using Newtonsoft.Json;
using System;

namespace SimplySecureApi.Data.Models.Domain.ViewModels
{
    public class LocationUserViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("locationId")]
        public Guid LocationId { get; set; }

        [JsonProperty("applicationUserId")]
        public Guid ApplicationUserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}