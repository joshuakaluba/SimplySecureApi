using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.PushNotificationTokens
{
    public class PushNotificationTokensRepository : BaseRepository, IPushNotificationTokensRepository
    {
        public async Task SavePushNotificationToken(PushNotificationToken token)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.PushNotificationTokens
                    .RemoveRange(DataContext.PushNotificationTokens
                        .Where(t => t.Token == token.Token));

                DataContext.PushNotificationTokens.Add(token);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task RemovePushNotificationToken(PushNotificationToken token)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.PushNotificationTokens
                    .RemoveRange(DataContext.PushNotificationTokens
                        .Where(t => t.Token == token.Token));

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<List<PushNotificationToken>> GetLocationsPushNotificationTokens(List<ApplicationUser> users)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var userIds = users.Select(c => c.Id).Distinct().ToList();

                var tokens
                    = await DataContext.PushNotificationTokens
                        .Where(n => userIds.Contains(n.ApplicationUserId))
                            .ToListAsync();

                return tokens;
            }
        }
    }
}