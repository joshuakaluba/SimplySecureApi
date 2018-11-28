using Newtonsoft.Json;
using System;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class BootMessage : Auditable
    {
        public BootMessage()
        {
        }

        public BootMessage(Guid moduleId, bool state)
        {
            ModuleId = moduleId;
            State = state;
        }

        [JsonProperty("state")]
        public bool State { get; set; }

        [JsonProperty("moduleId")]
        public Guid ModuleId { get; set; }

        public virtual Module Module { get; set; }
    }
}