using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Common.Exception;
using SimplySecureApi.Common.Extensions.Strings;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IModuleEventRepository _moduleEventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IModuleRepository _moduleRepository;

        public HomeController(UserManager<ApplicationUser> userManager,
            IModuleEventRepository moduleEventRepository,
            IModuleRepository moduleRepository,
            ILocationRepository locationRepository)
            : base(userManager)
        {
            _moduleEventRepository = moduleEventRepository;
            _locationRepository = locationRepository;
            _moduleRepository = moduleRepository;
        }

        public IActionResult Index()
        {
            try
            {
                var html = @" <!doctype html>
                        <html lang='en'>
                          <head>
                            <!-- Required meta tags -->
                            <meta charset='utf-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
                            <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css'>
                            <title>SimplySecureApi</title>
                          </head>
                          <body>
                            <div class='container'>
                                <h1 class='text-center' style='padding-top:150px;'>The SimplySecureApi.Web server is running!</h1>

                                
                            </div>
                            <script src='https://code.jquery.com/jquery-3.3.1.slim.min.js'></script>
                            <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js'></script>
                            <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js'></script>
                          </body>
                        </html>";

                return Content(html, "text/html", Encoding.UTF8);
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

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateNewLocation([FromBody] LocationViewModel locationViewModel)
        {
            try
            {
                var location = new Location
                {
                    Id = new Guid(),
                    Name = locationViewModel.Name.ToTitleCase(),
                    IsSilentAlarm = locationViewModel.IsSilentAlarm
                };

                await _locationRepository.CreateLocation(location);

                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> UpdateLocation([FromBody] LocationViewModel locationViewModel)
        {
            try
            {
                var location = await _locationRepository.FindLocationById(locationViewModel.Id);

                if (location == null)
                {
                    return BadRequest(new ErrorMessage("Unable to find location."));
                }

                location.Name = locationViewModel.Name.ToTitleCase();
                location.Armed = locationViewModel.Armed;
                location.IsSilentAlarm = locationViewModel.IsSilentAlarm;

                await _locationRepository.UpdateLocation(location);

                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                var locations = await _locationRepository.GetLocations();

                return Ok(locations);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetLocation(Guid id)
        {
            try
            {
                var location
                    = await _locationRepository.FindLocationById(id);

                if (location == null)
                {
                    return BadRequest(new ErrorMessage("Unable to find location."));
                }

                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetModulesByLocation(Guid id)
        {
            try
            {
                var location
                    = await _locationRepository.FindLocationById(id);

                if (location == null)
                {
                    return BadRequest(new ErrorMessage("Unable to find location."));
                }

                var modules
                    = await _moduleRepository
                        .GetModulesByLocation(location);

                return Ok(modules);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> DeleteLocation(Guid id)
        {
            try
            {
                var location
                    = await _locationRepository.FindLocationById(id);

                if (location == null)
                {
                    return BadRequest(new ErrorMessage("Unable to find location."));
                }

                await _locationRepository.DeleteLocation(location);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}