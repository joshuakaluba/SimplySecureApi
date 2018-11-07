using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Boots;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Authentication;

namespace SimplySecureApi.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected SimplySecureDataContext DbContext;

        protected readonly UserManager<ApplicationUser> UserManager;

        protected IBootRepository BootRepository;

        protected IModuleRepository ModuleRepository;

        protected BaseController(UserManager<ApplicationUser> userManager, SimplySecureDataContext dbContext)
        {
            this.UserManager = userManager;
            DbContext = dbContext;
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