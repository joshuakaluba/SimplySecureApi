using Microsoft.AspNetCore.Identity;
using SimplySecureApi.Common.Extensions.Strings;
using SimplySecureApi.Data.Initialization;
using SimplySecureApi.Data.Models.Authentication;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.DataAccessLayer.Authentication
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public async Task<IdentityResult> RegisterNewUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
        {
            user.FullName = user.FullName.ToTitleCase();

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, DataContextInitializer.DefaultUserClaim);

                await userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }
    }
}