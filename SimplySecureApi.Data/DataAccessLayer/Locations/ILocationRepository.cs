using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Locations
{
    public interface ILocationRepository
    {
        Task CreateLocation(Location location);

        Task TriggerLocation(Location location);

        Task ArmLocation(Location location);

        Task DisarmLocation(Location location);

        Task<Location> FindLocationById(Guid id);

        Task DeleteLocation(Location location);

        Task<List<Location>> GetLocations();

        Task UpdateLocation(Location location);
    }
}