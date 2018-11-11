using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using System.Diagnostics;

namespace SimplySecureApi.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserManager<ApplicationUser> userManager)
            : base(userManager)
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