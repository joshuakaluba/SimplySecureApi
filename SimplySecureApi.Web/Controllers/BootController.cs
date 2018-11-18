using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimplySecureApi.Data.DataAccessLayer.Boots;
using SimplySecureApi.Data.DataAccessLayer.Modules;
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
    public class BootController : BaseController
    {
        public BootController(UserManager<ApplicationUser> userManager, IBootRepository bootRepository, IModuleRepository moduleRepository)
            : base(userManager)
        {
            BootRepository = bootRepository;

            ModuleRepository = moduleRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaseComponentViewModel bootViewModel)
        {
            try
            {
                var module
                    = await ModuleRepository.FindModule
                        (Guid.Parse(bootViewModel.ModuleId));

                if (module == null)
                {
                    return BadRequest(new Exception("Invalid module id"));
                }

                var bootMessage = new BootMessage
                {
                    ModuleId = Guid.Parse(bootViewModel.ModuleId),

                    State = bootViewModel.State
                };

                await BootRepository.SaveBootMessage(bootMessage);

                var triggeredFlag = false;

                if (module.Armed || module.Triggered)
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