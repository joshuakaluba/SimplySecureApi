using Microsoft.AspNetCore.Identity;
using SimplySecureApi.Data.Models.Authentication;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Authentication
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterNewUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string password);
    }
}