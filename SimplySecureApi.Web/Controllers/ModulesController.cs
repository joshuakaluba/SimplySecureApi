using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Response;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class ModulesController : BaseController
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ILocationRepository _locationRepository;

        public ModulesController(UserManager<ApplicationUser> userManager, IModuleRepository moduleRepository, ILocationRepository locationRepository)
            : base(userManager)
        {
            _moduleRepository = moduleRepository;

            _locationRepository = locationRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var modules = await _moduleRepository.GetAllModules();

                return View(modules);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var locations = await _locationRepository.GetLocations();

                ViewData["LocationId"] = new SelectList(locations, "Id", "Name");

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,State,IsMotionDetector,LocationId")] Module module)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    module.Id = Guid.NewGuid();

                    await _moduleRepository.CreateModule(module);

                    TempData["CustomResponseAlert"] = CustomResponseAlert.GetStringResponse(ResponseStatusEnum.Success, $"{module.Name} module created successfully.");

                    return RedirectToAction(nameof(Index));
                }

                var locations = await _locationRepository.GetLocations();

                ViewData["LocationId"] = new SelectList(locations, "Id", "Name", module.LocationId);

                return View(module);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var module = await _moduleRepository.FindModule((Guid)id);

                if (module == null)
                {
                    return NotFound();
                }

                var locations = await _locationRepository.GetLocations();

                ViewData["LocationId"] = new SelectList(locations, "Id", "Name", module.LocationId);

                return View(module);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,IsMotionDetector,LocationId,Id")] Module module)
        {
            try
            {
                if (id != module.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var dbModule = await _moduleRepository.FindModule(module.Id);

                    if (dbModule == null)
                    {
                        return NotFound();
                    }

                    dbModule.Name = module.Name;
                    dbModule.IsMotionDetector = module.IsMotionDetector;
                    dbModule.LocationId = module.LocationId;

                    await _moduleRepository.UpdateModule(dbModule);

                    TempData["CustomResponseAlert"] = CustomResponseAlert.GetStringResponse(ResponseStatusEnum.Success, $"{module.Name} module saved successfully.");

                    return RedirectToAction(nameof(Index));
                }

                var locations = await _locationRepository.GetLocations();

                ViewData["LocationId"] = new SelectList(locations, "Id", "Name", module.LocationId);

                return View(module);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var module
                    = await _moduleRepository.FindModule((Guid)id);

                if (module == null)
                {
                    return NotFound();
                }

                return View(module);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var module = await _moduleRepository.FindModule(id);

                if (module == null)
                {
                    return NotFound();
                }

                await _moduleRepository.DeleteModule(module);

                TempData["CustomResponseAlert"] = CustomResponseAlert.GetStringResponse(ResponseStatusEnum.Success, $"{module.Name} module deleted successfully.");

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }
    }
}