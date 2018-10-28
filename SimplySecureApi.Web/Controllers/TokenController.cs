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
using TokenOptions = SimplySecureApi.Data.Models.TokenOptions;

namespace SimplySecureApi.Web.Controllers
{
    public class TokenController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenOptions _tokenOptions;

        public TokenController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<TokenOptions> tokens)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenOptions = tokens.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Generate([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Could not create token");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("Could not create token");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest("Could not create token");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Issuer,
                claims: userClaims,
                expires: DateTime.Now.AddYears(5),
                signingCredentials: credentials);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Could not register");
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                await _signInManager.SignInAsync(user, isPersistent: false);

                var claim = new Claim("DefaultUserClaim", "DefaultUserAuthorization");

                var addClaimResult = await _userManager.AddClaimAsync(user, claim);

                return Ok();
            }
            else
            {
                return BadRequest("Unable to register user");
            }
        }
    }
}