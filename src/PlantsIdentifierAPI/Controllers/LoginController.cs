using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.Helpers;
using System.Linq;
using PlantsIdentifierAPI.Models;
using System.Security.Cryptography;

namespace PlantsIdentifierAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        readonly UserManager<ApplicationUser> _userManager;
        readonly SignInManager<ApplicationUser> _signInManager;
        readonly SigningConfigurations _signingConfigurations;
        readonly TokenConfigurations _tokenConfigurations;

        public LoginController([FromServices]UserManager<ApplicationUser> userManager,
                    [FromServices]SignInManager<ApplicationUser> signInManager,
                    [FromServices]SigningConfigurations signingConfigurations,
                    [FromServices]TokenConfigurations tokenConfigurations)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfigurations = signingConfigurations;
            _tokenConfigurations = tokenConfigurations;
        }

        // [HttpPost]
        // [ProducesResponseType(200)]
        // [ProducesResponseType(401)]
        // [ProducesResponseType(500)]
        // public IActionResult Register([FromBody] RegisterUser user)
        // {
        //     return Ok();
        // }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] RegisterDTO user)
        {
            if (user != null && !string.IsNullOrWhiteSpace(user.UserEmail))
            {
                var users = _userManager.Users.ToList();
                //Check if the user exists on the database
                var userIdentity = await _userManager.FindByEmailAsync(user.UserEmail);
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var loginResult = await _signInManager.CheckPasswordSignInAsync(userIdentity, user.Password, false);
                    if (!loginResult.Succeeded)
                        return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized, Constants.WRONGEMAILORPASSWORD);
                    else
                    {
                        return GenerateToken(userIdentity);
                    }
                }
                else
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized, Constants.WRONGEMAILORPASSWORD);
            }

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, Constants.BADREQUEST);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(string token, string refreshToken)
        {
            //TODO we need to check all the possible ways this breaks
            var principal = GetPrincipalFromExpiredToken(token);
            //Retrieving user from database
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user.RefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();

            //TODO We need to make this method more generic so that we only need to check this one method for a point of failure.
            ReplaceRefreshToken(user, newRefreshToken);
            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
        }

        void ReplaceRefreshToken(ApplicationUser user, string newRefreshToken)
        {
            //TODO we need to check if this returns an error or not with a try catch
            user.RefreshToken = newRefreshToken;
            _userManager.UpdateAsync(user);
        }

        IActionResult GenerateToken(ApplicationUser userIdentity)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(userIdentity.UserName, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userIdentity.UserName),
                        new Claim("Email", userIdentity.Email)
                    }
                );

            DateTime createDate = DateTime.Now;
            DateTime expiryDate = createDate + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfigurations.Issuer,
                Audience = _tokenConfigurations.Audience,
                SigningCredentials = _signingConfigurations.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expiryDate
            });
            var token = handler.WriteToken(securityToken);

            return Ok(new
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expiryDate.ToString("yyyy-MM-dd HH:mm:ss"),
                accessToken = token
            });
        }

        string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        internal ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingConfigurations.Key,
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

    }
}