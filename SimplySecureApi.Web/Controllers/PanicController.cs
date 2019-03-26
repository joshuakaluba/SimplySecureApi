using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.Panics;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Services;
using SimplySecureApi.Data.Services.Messaging;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PanicController : BaseController<PanicController>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IPanicRepository _panicRepository;
        private readonly IMessagingService _messagingService;
        private readonly ILocationActionEventsRepository _locationActionEventsRepository;

        public PanicController(
            UserManager<ApplicationUser> userManager,
            ILocationRepository locationRepository,
            IPanicRepository panicRepository,
            IMessagingService messagingService,
            ILocationActionEventsRepository locationActionEventsRepository,
            ILogger<PanicController> logger)
            : base(userManager, logger)
        {
            _locationRepository = locationRepository;
            _panicRepository = panicRepository;
            _locationActionEventsRepository = locationActionEventsRepository;
            _messagingService = messagingService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> Post([FromBody] PanicViewModel panicViewModel)
        {
            try
            {
                var user = await GetUser();

                var panic = new Panic
                {
                    ApplicationUserId = user.Id,

                    Id = panicViewModel.Id,

                    DateCreated = DateTime.UtcNow
                };

                await _panicRepository.CreatePanic(panic);

                await PanickingService.SendPanicAlarm(_locationActionEventsRepository, _messagingService, _locationRepository, user);

                return Ok(panicViewModel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}