using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Response;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.Services
{
    public class LocationTriggeringService
    {
        public static async Task<ModuleResponse> ProcessLocationTriggered(ILocationRepository locationRepository, Location location)
        {
            var triggeredFlag = false;

            if (location.Armed)
            {
                await locationRepository.TriggerLocation(location);

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