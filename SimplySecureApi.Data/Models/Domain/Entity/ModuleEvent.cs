using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using SimplySecureApi.Common.Extensions.Duration;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class ModuleEvent : Auditable
    {
        public ModuleEvent()
        {
        }

        public ModuleEvent(Guid moduleId, bool state)
        {
            ModuleId = moduleId;

            State = state;
        }

        [Display(Name = "State")]
        public string StateDisplayed
        {
            get
            {
                if (Module == null)
                {
                    return State.ToString();
                }

                return 
                    Module.IsMotionDetector ? State ? "Motion detected" :
                    "No motion detected" :
                    State ? "Door Closed" : "Door Opened";
            }
        }

        [Display(Name = "State")]
        [JsonProperty("state")]
        public bool State { get; set; }

        [Display(Name = "Module")]
        [JsonProperty("moduleId")]
        public Guid ModuleId { get; set; }

        public virtual Module Module { get; set; }
    }
}