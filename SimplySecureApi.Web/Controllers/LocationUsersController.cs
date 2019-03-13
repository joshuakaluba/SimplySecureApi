using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimplySecureApi.Common.Exception;
using SimplySecureApi.Data.DataAccessLayer.Locations;
using SimplySecureApi.Data.DataAccessLayer.LocationUsers;
using SimplySecureApi.Data.DataAccessLayer.ModuleEvents;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Domain.ViewModels;
using SimplySecureApi.Data.Models.Response;
using SimplySecureApi.Data.Models.Static;
using System;
using System.Threading.Tasks;

namespace SimplySecureApi.Web.Controllers
{
    public class LocationUsersController : BaseController<LocationUsersController>
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILocationUsersRepository _locationUsersRepository;

        public LocationUsersController(UserManager<ApplicationUser> userManager,
            IModuleEventRepository moduleEventRepository,
            ILocationUsersRepository locationUsersRepository,
            ILocationRepository locationRepository,
            ILogger<LocationUsersController> logger)
            : base(userManager, logger)
        {
            _locationRepository = locationRepository;
            _locationUsersRepository = locationUsersRepository;
        }

        //LocationUsers/GetLocationUsers/{id}
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetLocationUsers(Guid id)
        {
            try
            {
                var user = await GetUser();

                var location = await _locationRepository.GetLocationById(id);

                await _locationRepository.ValidateLocationForUser(user, location);

                var locationUsers
                    = await _locationUsersRepository.GetLocationUsers(location);

                return Ok(locationUsers);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //LocationUsers/CreateNewLocationUser
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> CreateNewLocationUser([FromBody] LocationUserViewModel locationUserViewModel)
        {
            try
            {
                var user = await GetUser();

                var location = await _locationRepository.GetLocationById(locationUserViewModel.LocationId);

                if (location.CheckUserAdmin(user) == false)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnAuthorizedModifyLocation));
                }

                var userToAdd = await UserManager.FindByEmailAsync(locationUserViewModel.Email);

                if (userToAdd == null)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToFindUser));
                }

                await _locationUsersRepository.CreateLocationUser(location, userToAdd);

                return Ok(userToAdd);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorResponse(ex));
            }
        }

        //LocationUsers/DeleteLocationUser/{id}
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> DeleteLocationUser(Guid id)
        {
            try
            {
                var userToDelete
                    = await _locationUsersRepository.FindLocationUser(id);

                if (userToDelete == null)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToFindUser));
                }

                if (userToDelete.ApplicationUserId == userToDelete.Location.ApplicationUserId)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToDeleteAdmin));
                }

                var user = await GetUser();

                if (userToDelete.Location.CheckUserAdmin(user) == false)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnAuthorizedModifyLocation));
                }

                await _locationUsersRepository.DeleteLocationUser(userToDelete);

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