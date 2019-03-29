using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Notification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.PushNotificationTokens
{
    public interface IPushNotificationTokensRepository
    {
        Task SavePushNotificationToken(PushNotificationToken token);

        Task RemovePushNotificationToken(PushNotificationToken token);

        Task<List<PushNotificationToken>> GetLocationsPushNotificationTokens(List<ApplicationUser> users);
    }
}