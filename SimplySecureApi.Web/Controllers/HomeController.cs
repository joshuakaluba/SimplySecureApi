using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IModuleEventRepository _moduleEventRepository;

        public HomeController(UserManager<ApplicationUser> userManager, IModuleEventRepository moduleEventRepository)
            : base(userManager)
        {
            _moduleEventRepository = moduleEventRepository;
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

                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> ModuleEvents()
        {
            try
            {
                var modules
                    = await _moduleEventRepository.GetModuleEvents();

                return View(modules);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}