using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Data.Models.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public abstract class BaseController<T> : Controller
    {
        protected readonly UserManager<ApplicationUser> UserManager;
        protected ILogger<T> Logger;

        protected BaseController(UserManager<ApplicationUser> userManager, ILogger<T> logger)
        {
            UserManager = userManager;
            Logger = logger;
        }

        protected string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        protected async Task<ApplicationUser> GetUser()
        {
            var user = await UserManager.FindByIdAsync(GetUserId());
            return user;
        }
    }
}