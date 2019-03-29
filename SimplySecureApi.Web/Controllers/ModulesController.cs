using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Common.Exception;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Models.Static;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class ModulesController : BaseController<ModulesController>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleEventRepository _moduleEventRepository;

        public ModulesController(UserManager<ApplicationUser> userManager,
            IModuleRepository moduleRepository,
            IModuleEventRepository moduleEventRepository,
            ILocationRepository locationRepository,
            ILogger<ModulesController> logger)
            : base(userManager, logger)
        {
            _locationRepository = locationRepository;
            _moduleRepository = moduleRepository;
            _moduleEventRepository = moduleEventRepository;
        }

        //Modules/GetModulesByLocation/{id}
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
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Modules/GetModuleEventsByLocation/{id}
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetModuleEventsByLocation(Guid id)
        {
            try
            {
                var location = await _locationRepository.GetLocationById(id);

                var user = await GetUser();

                await _locationRepository.ValidateLocationForUser(user, location);

                var moduleEvents
                    = await _moduleEventRepository.GetModuleEventsByLocation(location);

                return Ok(moduleEvents);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Modules/CreateNewModule
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateNewModule([FromBody] ModuleViewModel moduleViewModel)
        {
            try
            {
                var user = await GetUser();

                var location = await _locationRepository.GetLocationById(moduleViewModel.LocationId);

                if (location.ApplicationUserId != user.Id)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnAuthorizedModifyLocation));
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
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //Modules/DeleteModule/{id}
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> DeleteModule(Guid id)
        {
            try
            {
                var module
                    = await _moduleRepository.FindModule(id);

                if (module == null)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToFindModule));
                }
                await _moduleRepository.DeleteModule(module);

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