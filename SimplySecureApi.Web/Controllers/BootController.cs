using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Boots;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Services;
using System;
using System.Threading.Tasks;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.Services.Messaging;

namespace SimplySecureApi.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BootController : BaseController
    {
        private readonly IBootRepository _bootRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationActionEventsRepository _locationActionEventsRepository;
        private readonly IMessagingService _messagingService;

        public BootController(
            UserManager<ApplicationUser> userManager, 
            IBootRepository bootRepository, 
            IModuleRepository moduleRepository,
            ILocationRepository locationRepository, 
            IMessagingService messagingService, 
            ILocationActionEventsRepository locationActionEventsRepository)
            : base(userManager)
        {
            _bootRepository = bootRepository;

            _moduleRepository = moduleRepository;

            _locationRepository = locationRepository;

            _locationActionEventsRepository = locationActionEventsRepository;

            _messagingService = messagingService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaseComponentViewModel bootViewModel)
        {
            try
            {
                var module
                    = await _moduleRepository.FindModule
                        (Guid.Parse(bootViewModel.ModuleId));

                if (module == null)
                {
                    return BadRequest(new Exception("Invalid module id"));
                }

                await _bootRepository
                    .SaveBootMessage
                        (new BootMessage(module.Id, bootViewModel.State));

                var moduleResponse
                    = await LocationTriggeringService
                        .DetermineIfTriggering(_locationRepository, _locationActionEventsRepository, _messagingService, module);

                module.State = bootViewModel.State;

                await _moduleRepository.UpdateModuleLastBoot(module);

                return Ok(moduleResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}