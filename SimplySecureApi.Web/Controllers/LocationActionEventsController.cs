using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Response;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class LocationActionEventsController : BaseController<LocationActionEventsController>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationActionEventsRepository _locationActionEventsRepository;

        public LocationActionEventsController(UserManager<ApplicationUser> userManager,
            ILocationActionEventsRepository locationActionEventsRepository,
            ILocationRepository locationRepository,
            ILogger<LocationActionEventsController> logger)
            : base(userManager, logger)
        {
            _locationRepository = locationRepository;
            _locationActionEventsRepository = locationActionEventsRepository;
        }

        //LocationActionEvents/GetLocationHistory/{id}
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
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}