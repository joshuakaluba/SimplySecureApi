using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimplySecureApi.Common.Exception;
using SimplySecureApi.Common.Extensions.Strings;
using SimplySecureApi.Data.DataAccessLayer.Authentication;
using SimplySecureApi.Data.DataAccessLayer.PushNotificationTokens;
using SimplySecureApi.Data.Models.Authentication;
using SimplySecureApi.Data.Models.Notification;
using SimplySecureApi.Data.Models.Static;
using SimplySecureApi.Services.Authentication;
using System;
using System.Threading.Tasks;
using TokenOptions = SimplySecureApi.Data.Models.Authentication.TokenOptions;

namespace SimplySecureApi.Web.Controllers
{
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenOptions _tokenOptions;
        private readonly IUserRepository _userRepository;
        private readonly IPushNotificationTokensRepository _pushNotificationTokensRepository;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            IPushNotificationTokensRepository pushNotificationTokensRepository,
            IOptions<TokenOptions> tokens,
            ILogger<AuthenticationController> logger)
            : base(userManager, logger)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenOptions = tokens.Value;
            _pushNotificationTokensRepository = pushNotificationTokensRepository;
        }

        //Authentication/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.IncompleteDataReceived));
                }

                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToLogIn));
                }

                var isUserValid = UserValidator.Validate(user);

                if (isUserValid == false)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.AccountDeactivated));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToLogIn));
                }

                var token = await TokenGenerator.Create(user, UserManager, _tokenOptions);

                return Ok(token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Authentication/Register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.IncompleteDataReceived));
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.FullName.ToTitleCase()
                };

                var result = await _userRepository.RegisterNewUser(UserManager, user, model.Password);

                if (result.Succeeded == false)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToRegister));
                }

                var token = await TokenGenerator.Create(user, UserManager, _tokenOptions);

                return Ok(token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Authentication/RegisterPushNotifications
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> RegisterPushNotifications([FromBody] PushNotificationModel model)
        {
            try
            {
                var user = await GetUser();

                var pushNotificationToken = new PushNotificationToken
                {
                    Token = model.Token,

                    ApplicationUserId = user.Id
                };

                await _pushNotificationTokensRepository.SavePushNotificationToken(pushNotificationToken);

                return Ok(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Authentication/UnRegisterPushNotifications
        [HttpPost]
        public async Task<IActionResult> UnRegisterPushNotifications([FromBody] PushNotificationModel model)
        {
            try
            {
                var pushNotificationToken = new PushNotificationToken
                {
                    Token = model.Token
                };

                await _pushNotificationTokensRepository.RemovePushNotificationToken(pushNotificationToken);

                return Ok(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);

                return BadRequest(new ErrorMessage(ex));
            }
        }
    }
}