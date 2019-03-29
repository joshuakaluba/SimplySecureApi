using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Models.Static;
using SimplySecureApi.Data.Services;
using SimplySecureApi.Data.Services.Messaging;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ModuleEventsController : BaseController<ModuleEventsController>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleEventRepository _moduleEventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMessagingService _messagingService;
        private readonly ILocationActionEventsRepository _locationActionEventsRepository;

        public ModuleEventsController(
            UserManager<ApplicationUser> userManager,
            IModuleRepository moduleRepository,
            IModuleEventRepository moduleEventRepository,
            ILocationRepository locationRepository,
            IMessagingService messagingService,
            ILocationActionEventsRepository locationActionEventsRepository,
            ILogger<ModuleEventsController> logger)
            : base(userManager, logger)
        {
            _moduleRepository = moduleRepository;
            _moduleEventRepository = moduleEventRepository;
            _locationRepository = locationRepository;
            _locationActionEventsRepository = locationActionEventsRepository;
            _messagingService = messagingService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaseComponentViewModel moduleEventViewModel)
        {
            try
            {
                var module
                    = await _moduleRepository.FindModule
                        (Guid.Parse(moduleEventViewModel.ModuleId));

                if (module == null)
                {
                    return BadRequest(new ErrorResponse(ErrorMessageResponses.InvalidModuleId));
                }

                await _moduleRepository.UpdateModuleState
                    (module, moduleEventViewModel.State);

                await _moduleEventRepository
                    .SaveModuleEvent
                        (new ModuleEvent(module.Id, moduleEventViewModel.State));

                var moduleResponse
                    = await LocationTriggeringService
                        .DetermineIfTriggering(_locationRepository, _locationActionEventsRepository, _messagingService, module);

                return Ok(moduleResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}