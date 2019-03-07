using Newtonsoft.Json;
using System;

namespace SimplySecureApi.Data.Models.Domain.ViewModels
{
    public class PanicViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty("dateCreated")]
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}