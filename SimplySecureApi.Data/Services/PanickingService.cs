using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Services.Messaging;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.Services
{
    public class PanickingService
    {
        public static async Task SendPanicAlarm(
            ILocationActionEventsRepository locationActionEventsRepository,
            IMessagingService messagingService,
            ILocationRepository locationRepository,
            ApplicationUser user)
        {
            var locations = await locationRepository.GetLocationsForUser(user);

            foreach (var location in locations)
            {
                var locationActionEvent = new LocationActionEvent
                {
                    LocationId = location.Id,

                    Action = LocationActionEnum.Panic,

                    ApplicationUserId = user.Id
                };

                await locationActionEventsRepository.SaveLocationActionEvent(locationActionEvent);

                await messagingService.SendPanicAlarm(location, user);
            }
        }
    }
}