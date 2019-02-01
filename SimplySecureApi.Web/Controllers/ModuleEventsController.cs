using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
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
    public class ModuleEventsController : BaseController
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
            ILocationActionEventsRepository locationActionEventsRepository)
            : base(userManager)
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
                    return BadRequest(new ErrorResponse(new Exception("Invalid module id")));
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
                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}