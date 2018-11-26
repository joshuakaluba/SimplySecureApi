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

namespace SimplySecureApi.Web.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class BootController : BaseController
    {
        public BootController(UserManager<ApplicationUser> userManager, IBootRepository bootRepository, IModuleRepository moduleRepository, ILocationRepository locationRepository)
            : base(userManager)
        {
            BootRepository = bootRepository;

            ModuleRepository = moduleRepository;

            LocationRepository = locationRepository;
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

                await BootRepository
                    .SaveBootMessage
                        (new BootMessage(module.Id, bootViewModel.State));

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