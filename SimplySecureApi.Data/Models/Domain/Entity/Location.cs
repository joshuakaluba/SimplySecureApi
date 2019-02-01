using Newtonsoft.Json;
using SimplySecureApi.Data.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace SimplySecureApi.Data.Models.Domain.Entity
{
    public class Location : Auditable
    {
        [Display(Name = "Location Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display(Name = "Armed")]
        [JsonProperty("armed")]
        public bool Armed { get; internal set; }

        [Display(Name = "Silent Alarm")]
        [JsonProperty("isSilentAlarm")]
        public bool IsSilentAlarm { get; set; }

        [Display(Name = "Triggered")]
        [JsonProperty("triggered")]
        [ScaffoldColumn(false)]
        public bool Triggered { get; internal set; }

        public bool IsNotTriggered => !Triggered;

        [JsonProperty("status")]
        public string Status => Triggered ? "Triggered" : Armed ? "Armed" : "Disarmed";

        [JsonProperty("active")]
        public bool Active { get; set; } = true;

        public bool CheckUserAdmin(ApplicationUser user)
        {
            return ApplicationUserId.Equals(user.Id);
        }

        [ScaffoldColumn(false)]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}