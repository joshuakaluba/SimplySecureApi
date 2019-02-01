using System.Collections.Generic;
using SimplySecureApi.Data.Models.Notification;
using System.Threading.Tasks;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;

namespace SimplySecureApi.Data.DataAccessLayer.PushNotificationTokens
{
    public interface IPushNotificationTokensRepository
    {
        Task SavePushNotificationToken(PushNotificationToken token);
        Task RemovePushNotificationToken(PushNotificationToken token);
        Task<List<PushNotificationToken>> GetLocationsPushNotificationTokens(List<ApplicationUser> users);
    }
}