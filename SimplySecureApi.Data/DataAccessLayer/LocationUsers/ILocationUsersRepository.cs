using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;

namespace SimplySecureApi.Data.DataAccessLayer.LocationUsers
{
    public interface ILocationUsersRepository
    {
        Task CreateLocationUser(Location location, ApplicationUser user);

        Task CreateLocationUser(LocationUser locationUser);

        Task DeleteLocationUser(LocationUser locationUser);

        Task<LocationUser> FindLocationUser(Guid id);

        Task<List<LocationUser>> GetLocationUsers(Location location);

        //Task<bool> ValidateUserLocation
    }
}
