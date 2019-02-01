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
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.LocationUsers;

namespace SimplySecureApi.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IModuleEventRepository _moduleEventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly ILocationUsersRepository _locationUsersRepository;
        private readonly ILocationActionEventsRepository _locationActionEventsRepository;

        public HomeController(UserManager<ApplicationUser> userManager,
            IModuleEventRepository moduleEventRepository,
            IModuleRepository moduleRepository,
            ILocationUsersRepository locationUsersRepository,
            ILocationActionEventsRepository locationActionEventsRepository,
            ILocationRepository locationRepository)
            : base(userManager)
        {
            _moduleEventRepository = moduleEventRepository;

            _locationRepository = locationRepository;

            _moduleRepository = moduleRepository;

            _locationActionEventsRepository = locationActionEventsRepository;

            _locationUsersRepository = locationUsersRepository;
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

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetModuleEventsByLocation(Guid id)
        {
            try
            {
                var location = await _locationRepository.GetLocationById(id);

                var moduleEvents
                    = await _moduleEventRepository.GetModuleEventsByLocation(location);

                return Ok(moduleEvents);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetLocationUsers(Guid id)
        {
            try
            {
                var location = await _locationRepository.GetLocationById(id);

                var user = await GetUser();

                await _locationRepository.ValidateLocationForUser(user, location);
                
                var locationUsers 
                    = await _locationUsersRepository.GetLocationUsers(location);
                
                return Ok(locationUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpGet]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateNewLocation([FromBody] LocationViewModel locationViewModel)
        {
            try
            {
                var user = await GetUser();

                var location = new Location
                {
                    Id = new Guid(),
                    Name = locationViewModel.Name.ToTitleCase(),
                    IsSilentAlarm = locationViewModel.IsSilentAlarm,
                    ApplicationUserId = user.Id
                };

                await _locationRepository.CreateLocation(_locationUsersRepository,location);

                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateNewModule([FromBody] ModuleViewModel moduleViewModel)
        {
            try
            {
                var user = await GetUser();

                var location = await _locationRepository.GetLocationById(moduleViewModel.LocationId);

                if (location.CheckUserAdmin(user) == false)
                {
                    return BadRequest(new ErrorMessage("You are unauthorized to add new modules at this location. Only admins can add new modules"));
                }

                var module = new Module
                {
                    LocationId = moduleViewModel.LocationId,
                    Name = moduleViewModel.Name,
                    IsMotionDetector = moduleViewModel.IsMotionDetector
                };

                await _moduleRepository.CreateModule(module);

                return Ok(moduleViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateNewLocationUser([FromBody] LocationUserViewModel locationUserViewModel)
        {
            try
            {
                var user = await GetUser();

                var location = await _locationRepository.GetLocationById(locationUserViewModel.LocationId);

                if (location.CheckUserAdmin(user) == false)
                {
                    return BadRequest(new ErrorMessage("You are unauthorized to add new users at this location. Only admins can add new users"));
                }

                var userToAdd = await UserManager.FindByEmailAsync(locationUserViewModel.Email);

                if (userToAdd == null)
                {
                    return BadRequest(new ErrorMessage("Unable to find user. Please have your user register"));
                }

                await _locationUsersRepository.CreateLocationUser(location, userToAdd);
                
                return Ok(userToAdd);
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
                var location = await _locationRepository.GetLocationById(locationViewModel.Id);

                location.Name = locationViewModel.Name.ToTitleCase();
                location.IsSilentAlarm = locationViewModel.IsSilentAlarm;

                await _locationRepository.UpdateLocation(location);

                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> ArmLocation([FromBody] LocationViewModel locationViewModel)
        {
            try
            {
                var location = await _locationRepository.GetLocationById(locationViewModel.Id);

                var user = await GetUser();

                await _locationRepository.ValidateLocationForUser(user, location);

                if (locationViewModel.Armed)
                {
                    await _locationRepository.ArmLocation(location, user, _locationActionEventsRepository);
                }
                else
                {
                    await _locationRepository.DisarmLocation(location, user, _locationActionEventsRepository);
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
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                var user = await GetUser();

                var locations = await _locationRepository.GetLocationsForUser(user);

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
                    = await _locationRepository.GetLocationById(id);

                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetLocationHistory(Guid id)
        {
            try
            {
                var location
                    = await _locationRepository.GetLocationById(id);

                var user = await GetUser();

                await _locationRepository.ValidateLocationForUser(user, location);

                var events 
                    = await _locationActionEventsRepository.GetLocationActionEventsByLocation(location);

                return Ok(events);
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
                    = await _locationRepository.GetLocationById(id);

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
                    = await _locationRepository.GetLocationById(id);

                var user = await GetUser();

                if (!location.CheckUserAdmin(user))
                {
                    return BadRequest(new ErrorMessage("You are unauthorized to modify this location. Only admin can add modify this location"));
                }

                await _locationRepository.DeleteLocation(location);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> DeleteLocationUser(Guid id)
        {
            try
            {
                var locationUser
                    = await _locationUsersRepository.FindLocationUser(id);

                if (locationUser == null)
                {
                    return BadRequest(new ErrorMessage("Unable to find location user."));
                }

                if (locationUser.ApplicationUserId == locationUser.Location.ApplicationUserId)
                {
                    return BadRequest(new ErrorMessage($"You can not delete the location admin"));
                }

                var user = await GetUser();

                if (locationUser.Location.CheckUserAdmin(user) == false)
                {
                    return BadRequest(new ErrorMessage("You are unauthorized to modify users at this location. Only admins can modify users"));
                }

                await _locationUsersRepository.DeleteLocationUser(locationUser);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> DeleteModule(Guid id)
        {
            try
            {
                var module
                    = await NewMethod(id);

                if (module == null)
                {
                    return BadRequest(new ErrorMessage("Unable to find module."));
                }

                var user = await GetUser();

                if (module.Location.CheckUserAdmin(user) == false)
                {
                    return BadRequest(new ErrorMessage("You are unauthorized to modify users at this location. Only admins can modify modules"));
                }

                await _moduleRepository.DeleteModule(module);

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        private Task<Module> NewMethod(Guid id)
        {
            return _moduleRepository.FindModule(id);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}