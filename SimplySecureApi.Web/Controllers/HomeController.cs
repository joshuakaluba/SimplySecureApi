using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models;
using SimplySecureApi.Data.Models.Authentication;
using System.Diagnostics;

namespace SimplySecureApi.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager, SimplySecureDataContext dbContext)
            : base(userManager, dbContext)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}