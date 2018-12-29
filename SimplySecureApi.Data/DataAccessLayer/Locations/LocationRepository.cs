using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Locations
{
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public async Task CreateLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.Locations.Add(location);

                await DataContext.SaveChangesAsync();
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

        public async Task ArmLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                location.Armed = true;

                location.Triggered = false;

                DataContext.Locations.Update(location);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task DisarmLocation(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                location.Armed = false;

                location.Triggered = false;

                DataContext.Locations.Update(location);

                await DataContext.SaveChangesAsync();
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

        public async Task<List<Location>> GetLocations()
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var locations
                    = await DataContext.Locations
                        .OrderBy(l => l.Name)
                            .ToListAsync();

                return locations;
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