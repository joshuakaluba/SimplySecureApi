using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SimplySecureApi.Data.Models.Authentication;
using TokenOptions = SimplySecureApi.Data.Models.Authentication.TokenOptions;

namespace SimplySecureApi.Services.Authentication
{
    public static class TokenGenerator
    {
        public static async Task<Token> Create(ApplicationUser user, UserManager<ApplicationUser> userManager, TokenOptions tokenOptions)
        {
            var userClaims = await userManager.GetClaimsAsync(user);

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));

            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryDate = DateTime.Now.AddYears(3);

            var jwsSecurityToken = new JwtSecurityToken(
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Issuer,
                claims: userClaims,
                expires: expiryDate,
                signingCredentials: creds);

            return new Token
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwsSecurityToken),

                FullName = user.FullName,

                Email = user.Email,

                PhoneNumber = user.PhoneNumber,

                UserId = user.Id,

                ExpiryDate = expiryDate,

                DateUserCreated = user.DateCreated
            };
        }
    }
}
