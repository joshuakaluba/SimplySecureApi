using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Response;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class LocationsController : BaseController
    {
        private readonly ILocationRepository _locationRepository;

        public LocationsController(UserManager<ApplicationUser> userManager, ILocationRepository locationRepository)
            : base(userManager)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var locations
                    = await _locationRepository.GetLocations();

                return View(locations);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsSilentAlarm")] Location location)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    location.Id = Guid.NewGuid();
                    location.Armed = false;
                    location.Triggered = false;
                    location.Active = true;

                    await _locationRepository.CreateLocation(location);

                    TempData["CustomResponseAlert"] = CustomResponseAlert.GetStringResponse(ResponseStatusEnum.Success, $"{location.Name} created successfully.");

                    return RedirectToAction(nameof(Index));
                }
                return View(location);
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

                var location
                    = await _locationRepository.FindLocationById((Guid)id);

                if (location == null)
                {
                    return NotFound();
                }

                return View(location);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Armed,IsSilentAlarm,Id")] Location location)
        {
            try
            {
                if (id != location.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var armed = location.Armed;

                    var dbLocation = await _locationRepository.FindLocationById(location.Id);

                    if (dbLocation != null)
                    {
                        dbLocation.IsSilentAlarm = location.IsSilentAlarm;

                        if (armed)
                        {
                            await _locationRepository.ArmLocation(dbLocation);

                            TempData["CustomResponseAlert"] = CustomResponseAlert.GetStringResponse(ResponseStatusEnum.Success, $"{dbLocation.Name} successfully armed.");
                        }
                        else
                        {
                            await _locationRepository.DisarmLocation(dbLocation);

                            TempData["CustomResponseAlert"] = CustomResponseAlert.GetStringResponse(ResponseStatusEnum.Success, $"{dbLocation.Name} successfully disarmed.");
                        }
                    }
                    else
                    {
                        return NotFound();
                    }

                    return RedirectToAction(nameof(Index));
                }
                return View(location);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToAction("Error", "Home");
            }
        }
    }
}