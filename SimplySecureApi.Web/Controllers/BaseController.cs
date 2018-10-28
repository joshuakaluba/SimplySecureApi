using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Authentication;

namespace SimplySecureApi.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected SimplySecureDataContext _context;

        protected readonly UserManager<ApplicationUser> userManager;

        protected BaseController(UserManager<ApplicationUser> userManager, SimplySecureDataContext dbContext)
        {
            this.userManager = userManager;
            _context = dbContext;
        }

        protected string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return userId;
        }

        protected async Task<ApplicationUser> GetUser()
        {
            var user = await userManager.FindByIdAsync(GetUserId());

            return user;
        }
    }
}