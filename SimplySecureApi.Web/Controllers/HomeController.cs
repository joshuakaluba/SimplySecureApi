using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using System;
using System.Diagnostics;

namespace SimplySecureApi.Web.Controllers
{
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger)
            : base(userManager, logger)
        {
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                Logger.LogError(ex.Message);

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}