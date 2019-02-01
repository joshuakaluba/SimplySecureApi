using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.LocationUsers;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Locations
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public async Task CreateLocation(ILocationUsersRepository locationUsersRepository, Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Locations.Add(location);

                await DataContext.SaveChangesAsync();

                var locationUser = new LocationUser
                {
                    LocationId = location.Id,

                    ApplicationUserId = location.ApplicationUserId
                };

                await locationUsersRepository.CreateLocationUser(locationUser);
            }
        }

        public async Task TriggerLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                location.Triggered = true;

                DataContext.Locations.Update(location);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task ArmLocation(Location location, ApplicationUser user, ILocationActionEventsRepository locationActionEventsRepository)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                location.Armed = true;

                location.Triggered = false;

                DataContext.Locations.Update(location);

                await DataContext.SaveChangesAsync();

                var locationAction = new LocationActionEvent
                {
                    ApplicationUserId = user.Id,

                    Action = LocationActionEnum.Armed,

                    LocationId = location.Id
                };

                await locationActionEventsRepository.SaveLocationActionEvent(locationAction);
            }
        }

        public async Task DisarmLocation(Location location, ApplicationUser user, ILocationActionEventsRepository locationActionEventsRepository)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                location.Armed = false;

                location.Triggered = false;

                DataContext.Locations.Update(location);

                await DataContext.SaveChangesAsync();

                var locationAction = new LocationActionEvent
                {
                    ApplicationUserId = user.Id,

                    Action = LocationActionEnum.Disarmed,

                    LocationId = location.Id
                };

                await locationActionEventsRepository.SaveLocationActionEvent(locationAction);
            }
        }

        public async Task<Location> FindLocationById(Guid id)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var location = await DataContext.Locations.FindAsync(id);

                return location;
            }
        }

        public async Task DeleteLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Locations.Remove(location);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<List<Location>> GetLocationsForUser(ApplicationUser user)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var locations =
                    await DataContext.LocationUsers
                            .Where(m => m.ApplicationUserId == user.Id)
                                .Select(m => m.Location)
                                    .ToListAsync();
                return locations;
            }
        }

        public async Task ValidateLocationForUser(ApplicationUser user, Location location)
        {
            var userLocations = await GetLocationsForUser(user);

            var found = userLocations.Any(l => l.Id == location.Id);

            if (found == false)
            {
                throw new Exception("User is not authorized for current location");
            }

        }

        public async Task UpdateLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Locations.Update(location);

                await DataContext.SaveChangesAsync();
            }
        }
    }
}