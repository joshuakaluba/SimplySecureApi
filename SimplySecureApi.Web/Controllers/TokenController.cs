using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimplySecureApi.Data.Models.Authentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using SimplySecureApi.Common.Exception;
using SimplySecureApi.Data.DataAccessLayer.Authentication;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Services.Authentication;
using TokenOptions = SimplySecureApi.Data.Models.Authentication.TokenOptions;

namespace SimplySecureApi.Web.Controllers
{
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TokenController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenOptions _tokenOptions;
        private readonly IUserRepository _userRepository;

        public TokenController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            IOptions<TokenOptions> tokens,
            SimplySecureDataContext context) : base(userManager)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenOptions = tokens.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var unableTologIn = "Unable to log user in.";

                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessage("Incomplete data received."));
                }

                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return BadRequest(new ErrorMessage(unableTologIn));
                }

                var isUserValid = UserValidator.Validate(user);

                if (isUserValid == false)
                {
                    return BadRequest(new ErrorMessage("Account de-activated."));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                {
                    return BadRequest(new ErrorMessage(unableTologIn));
                }

                var token = await TokenGenerator.Create(user, UserManager, _tokenOptions);

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessage(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessage("Incomplete data received."));
                }

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, FullName = model.FullName };

                var result = await _userRepository.RegisterNewUser(UserManager, user, model.Password);

                if (result.Succeeded)
                {
                    var token = await TokenGenerator.Create(user, UserManager, _tokenOptions);

                    return Ok(token);
                }
                else
                {
                    return BadRequest(new ErrorMessage("Unable to register user."));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorMessage(ex));
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public IActionResult ValidateToken()
        {
            return Ok();
        }
    }
    //public class TokenController : Controller
    //{
    //    private readonly UserManager<ApplicationUser> _userManager;
    //    private readonly SignInManager<ApplicationUser> _signInManager;
    //    private readonly TokenOptions _tokenOptions;

    //    public TokenController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<TokenOptions> tokens)
    //    {
    //        _userManager = userManager;

    //        _signInManager = signInManager;

    //        _tokenOptions = tokens.Value;
    //    }

    //    [AllowAnonymous]
    //    [HttpPost]
    //    public async Task<IActionResult> Generate([FromBody] LoginViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest("Could not create token");
    //        }

    //        var user = await _userManager.FindByEmailAsync(model.Email);

    //        if (user == null)
    //        {
    //            return BadRequest("Could not create token");
    //        }

    //        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

    //        if (!result.Succeeded)
    //        {
    //            return BadRequest("Could not create token");
    //        }

    //        var userClaims = await _userManager.GetClaimsAsync(user);

    //        userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

    //        userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key));

    //        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(
    //            issuer: _tokenOptions.Issuer,
    //            audience: _tokenOptions.Issuer,
    //            claims: userClaims,
    //            expires: DateTime.Now.AddYears(5),
    //            signingCredentials: credentials);

    //        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    //    }

    //    [AllowAnonymous]
    //    [HttpPost]
    //    public async Task<IActionResult> Register(RegisterViewModel model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest("Could not register");
    //        }

    //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

    //        var result = await _userManager.CreateAsync(user, model.Password);

    //        if (result.Succeeded)
    //        {
    //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

    //            await _signInManager.SignInAsync(user, isPersistent: false);

    //            var claim = new Claim("DefaultUserClaim", "DefaultUserAuthorization");

    //            var addClaimResult = await _userManager.AddClaimAsync(user, claim);

    //            return Ok();
    //        }
    //        else
    //        {
    //            return BadRequest("Unable to register user");
    //        }
    //    }
    //}
}