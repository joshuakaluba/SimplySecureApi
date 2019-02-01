using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.LocationUsers;
using SimplySecureApi.Data.Models.Authentication;

namespace SimplySecureApi.Data.DataAccessLayer.Locations
{
    public interface ILocationRepository
    {
        Task CreateLocation(ILocationUsersRepository locationUsersRepository, Location location);

        Task TriggerLocation(Location location);

        Task ArmLocation(Location location, ApplicationUser user, ILocationActionEventsRepository locationActionEventsRepository);

        Task DisarmLocation(Location location, ApplicationUser user, ILocationActionEventsRepository locationActionEventsRepository);

        Task<Location> FindLocationById(Guid id);

        Task DeleteLocation(Location location);

        Task<List<Location>> GetLocationsForUser(ApplicationUser user);

        Task ValidateLocationForUser(ApplicationUser user, Location location);

        Task UpdateLocation(Location location);
    }
}