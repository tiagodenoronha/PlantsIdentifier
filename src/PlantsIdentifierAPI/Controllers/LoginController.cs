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

        private IActionResult GenerateToken(ApplicationUser userIdentity)
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
            DateTime expiryDate = createDate +
                TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

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
                accessToken = token,
            });
        }
    }
}