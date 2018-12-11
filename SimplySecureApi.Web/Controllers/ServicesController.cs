using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.Services.Messaging;

namespace SimplySecureApi.Web.Controllers
{
    public class ServicesController : BaseController
    {
        private readonly IModuleRepository _moduleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMessagingService _messagingService;

        public ServicesController(UserManager<ApplicationUser> userManager, IModuleRepository moduleRepository, ILocationRepository locationRepository, IMessagingService messagingService)
            : base(userManager)
        {
            _moduleRepository = moduleRepository;

            _locationRepository = locationRepository;

            _messagingService = messagingService;
        }

        [HttpPost]
        public async Task<IActionResult> SynchronizeModules([FromBody] List<ModuleViewModel> modules)
        {
            try
            {
                await _moduleRepository.UpdateModuleHeartbeats
                    (modules, _locationRepository, _messagingService);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOfflineModules()
        {
            try
            {
                await _moduleRepository.ProcessOfflineModules( _locationRepository, _messagingService);

                return Ok("Processed");
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}