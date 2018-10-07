using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.Helpers;

namespace PlantsIdentifierAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
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
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] RegisterDTO user)
        {
            if (user != null && !string.IsNullOrWhiteSpace(user.UserEmail))
            {
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
            }
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest, Constants.BADREQUEST);
        }

        private IActionResult GenerateToken(ApplicationUser userIdentity)
        {
            throw new NotImplementedException();
        }
    }
}