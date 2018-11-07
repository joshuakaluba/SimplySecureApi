using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    class TriggeredModule : Auditable
    {
        [JsonProperty("state")]
        public bool State { get; set; }

        [JsonProperty("moduleId")]
        public Guid ModuleId { get; set; }

        public virtual Module Module { get; set; }
    }
}
