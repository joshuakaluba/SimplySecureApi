using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace SimplySecureApi.Data.Models.Notification
{
    public class PushNotificationToken : Auditable
    {
        [ScaffoldColumn(false)]
        public string Token { get; set; }

        [ScaffoldColumn(false)]
        public bool Active { get; set; } = true;

        [ScaffoldColumn(false)]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public override bool Equals(Auditable other)
        {
            var token = (PushNotificationToken) other;

            return Token == token.Token;
        }
    }
}