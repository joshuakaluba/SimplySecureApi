using Newtonsoft.Json;
using SimplySecureApi.Common.Extensions.Duration;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Module : Auditable
    {
        [Display(Name = "Module Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display(Name = "State")]
        [JsonProperty("state")]
        public bool State { get; set; }

        [Display(Name = "Motion Detector")]
        [JsonProperty("isMotionDetector")]
        public bool IsMotionDetector { get; set; }

        [Display(Name = "Last Heartbeat")]
        [JsonProperty("lastHeartbeat")]
        public DateTime LastHeartbeat { get; set; } = DateTime.MinValue;

        [Display(Name = "Last Boot")]
        [JsonProperty("lastBoot")]
        public DateTime LastBoot { get; set; } = DateTime.MinValue;

        public string RelativeLastBoot => LastBoot.RelativeDate();

        public string RelativeLastHeartbeat => LastHeartbeat.RelativeDate();

        [Display(Name = "Offline")]
        [JsonProperty("offline")]
        public bool Offline { get; set; } = true;

        [Display(Name = "State")]
        public string StateDisplayed
        {
            get
            {
                return IsMotionDetector ? State ? "Motion detected" :
                    "No motion detected" :
                    State ? "Door Closed" : "Door Opened";
            }
        }

        [Display(Name = "Location")]
        [JsonProperty("locationId")]
        public Guid LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}