using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Modules;
using SimplySecureApi.Data.DataAccessLayer.StateChanges;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.Entity;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class StateChangeController : BaseController
    {
        public StateChangeController(UserManager<ApplicationUser> userManager, IModuleRepository moduleRepository, IStateChangesRepository stateChangesRepository)
            : base(userManager)
        {
            ModuleRepository = moduleRepository;

            StateChangesRepository = stateChangesRepository;
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

                var stateChange = new ModuleStateChange
                {
                    ModuleId = module.Id,

                    State = stateChangeViewModel.State
                };

                await StateChangesRepository.SaveStateChange(stateChange);

                var triggeredFlag = false;

                if (module.Armed)
                {
                    await ModuleRepository.TriggerModule(module);

                    triggeredFlag = true;
                }

                var moduleResponse = new ModuleResponse
                {
                    Armed = module.Armed,

                    Triggered = triggeredFlag
                };

                return Ok(moduleResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(ex));
            }
        }
    }
}