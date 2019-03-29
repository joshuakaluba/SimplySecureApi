using Microsoft.EntityFrameworkCore;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.LocationUsers
{
    public class LocationUsersRepository : BaseRepository, ILocationUsersRepository
    {
        public async Task CreateLocationUser(Location location, ApplicationUser user)
        {
            var locationUser
                = new LocationUser
                {
                    LocationId = location.Id,
                    ApplicationUserId = user.Id
                };

            await CreateLocationUser(locationUser);
        }

        public async Task CreateLocationUser(LocationUser locationUser)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.LocationUsers.RemoveRange
                    (DataContext.LocationUsers.Where(u => u.ApplicationUserId == locationUser.ApplicationUserId
                                                        && u.LocationId == locationUser.LocationId));

                DataContext.LocationUsers.Add(locationUser);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteLocationUser(LocationUser locationUser)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                DataContext.LocationUsers.Remove(locationUser);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<LocationUser> FindLocationUser(Guid id)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var locationUser
                    = await DataContext.LocationUsers
                        .Where(u => u.Id == id)
                            .Include(u => u.Location)
                                .Include(u => u.ApplicationUser)
                                    .FirstOrDefaultAsync();

                return locationUser;
            }
        }

        public async Task<List<LocationUser>> GetLocationUsers(Location location)
        {
            using (DataContext = new SimplySecureDataContext())
            {
                var locationUsers =
                    await DataContext.LocationUsers
                        .Where(l => l.LocationId == location.Id)
                            .Include(l => l.Location)
                                .Include(l => l.ApplicationUser)
                                    .OrderBy(m => m.Name)
                                        .ToListAsync();
                return locationUsers;
            }
        }

        public async Task<List<ApplicationUser>> GetLocationApplicationUsers(Location location)
        {
            var locationUsers = await GetLocationUsers(location);

            var applicationUsers = locationUsers.Select(m => m.ApplicationUser).ToList();

            return applicationUsers;
        }
    }
}