using SimplySecureApi.Data.Models.Authentication;

namespace SimplySecureApi.Services.Authentication
{
    public static class UserValidator
    {
        public static bool Validate(ApplicationUser user)
        {
            return user.Active;
        }
    }
}