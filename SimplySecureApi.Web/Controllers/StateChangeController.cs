using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.DataAccessLayer.StateChanges;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Services;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class StateChangeController : BaseController
    {
        public StateChangeController(UserManager<ApplicationUser> userManager, IModuleRepository moduleRepository, IStateChangeRepository stateChangeRepository, ILocationRepository locationRepository)
            : base(userManager)
        {
            ModuleRepository = moduleRepository;

            StateChangeRepository = stateChangeRepository;

            LocationRepository = locationRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaseComponentViewModel stateChangeViewModel)
        {
            try
            {
                var module
                    = await ModuleRepository.FindModule
                        (Guid.Parse(stateChangeViewModel.ModuleId));

                if (module == null)
                {
                    return BadRequest(new ErrorResponse(new Exception("Invalid module id")));
                }

                await ModuleRepository.UpdateModuleState(module, stateChangeViewModel.State);

                await StateChangeRepository
                    .SaveStateChange
                        (new ModuleStateChange(module.Id, stateChangeViewModel.State));

                var moduleResponse
                    = await LocationTriggeringService
                        .ProcessLocationTriggered(LocationRepository, module.Location);

                return Ok(moduleResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}