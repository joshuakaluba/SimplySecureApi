using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Services.Messaging;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.Services
{
    public class LocationTriggeringService
    {
        public static async Task<ModuleResponse> DetermineIfTriggering(
            ILocationRepository locationRepository,
            ILocationActionEventsRepository locationActionEventsRepository,
            IMessagingService messagingService,
            Module module)
        {
            var location = module.Location;

            var triggeredFlag = false;

            if (location.Armed && location.IsNotTriggered)
            {
                await locationRepository.TriggerLocation(location);

                await messagingService.SendModuleTriggeredMessage(module);

                var locationActionEvent = new LocationActionEvent
                {
                    LocationId = location.Id,

                    Action = LocationActionEnum.Triggered
                };

                await locationActionEventsRepository.SaveLocationActionEvent(locationActionEvent);

                triggeredFlag = true;
            }

            var moduleResponse = new ModuleResponse
            {
                Armed = location.Armed,

                Triggered = triggeredFlag && !location.IsSilentAlarm
            };

            return moduleResponse;
        }
    }
}