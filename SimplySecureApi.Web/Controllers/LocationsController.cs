using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Common.Exception;
using SimplySecureApi.Common.Extensions.Strings;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.LocationUsers;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Models.Static;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class LocationsController : BaseController<LocationsController>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationUsersRepository _locationUsersRepository;
        private readonly ILocationActionEventsRepository _locationActionEventsRepository;

        public LocationsController(UserManager<ApplicationUser> userManager,
            ILocationUsersRepository locationUsersRepository,
            ILocationActionEventsRepository locationActionEventsRepository,
            ILocationRepository locationRepository,
            ILogger<LocationsController> logger)
            : base(userManager, logger)
        {
            _locationRepository = locationRepository;
            _locationActionEventsRepository = locationActionEventsRepository;
            _locationUsersRepository = locationUsersRepository;
        }

        //Locations/GetLocations
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetLocations()
        {
            try
            {
                var user = await GetUser();

                var locations = await _locationRepository.GetLocationsForUser(user);

                return Ok(locations);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Locations/GetLocation/{id}
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetLocation(Guid id)
        {
            try
            {
                var user = await GetUser();

                var locations = await _locationRepository.GetLocationsForUser(user);

                var location = locations.Single(l => l.Id == id);

                if (location == null)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToFindLocation));
                }

                return Ok(location);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Locations/CreateLocation
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationViewModel locationViewModel)
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

                await _locationRepository.CreateLocation(_locationUsersRepository, location);

                return Ok(location);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Locations/ArmLocation
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
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Locations/DeleteLocation/{id}
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
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnAuthorizedModifyLocation));
                }

                await _locationRepository.DeleteLocation(location);

                return Ok(id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}