using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Data.DataAccessLayer.LocationActionEvents;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Services.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class ServicesController : BaseController<ServicesController>
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMessagingService _messagingService;
        private readonly ILocationActionEventsRepository _locationActionEventsRepository;

        public ServicesController(
            UserManager<ApplicationUser> userManager,
            IModuleRepository moduleRepository,
            ILocationRepository locationRepository,
            IMessagingService messagingService,
            ILocationActionEventsRepository locationActionEventsRepository,
            ILogger<ServicesController> logger)
            : base(userManager, logger)
        {
            _moduleRepository = moduleRepository;
            _locationRepository = locationRepository;
            _locationActionEventsRepository = locationActionEventsRepository;
            _messagingService = messagingService;
        }

        //Services/SynchronizeModules
        [HttpPost]
        public async Task<IActionResult> SynchronizeModules([FromBody] List<ModuleViewModel> modules)
        {
            try
            {
                await _moduleRepository.UpdateModuleHeartbeats
                    (modules, _locationRepository, _messagingService, _locationActionEventsRepository);

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Services/ProcessOfflineModules
        [HttpPost]
        public async Task<IActionResult> ProcessOfflineModules()
        {
            try
            {
                await _moduleRepository.ProcessOfflineModules(_locationRepository, _messagingService, _locationActionEventsRepository);

                return Ok("Processed");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}