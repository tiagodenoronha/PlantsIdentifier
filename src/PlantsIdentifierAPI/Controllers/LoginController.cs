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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult Register([FromBody] RegisterUser user)
        {
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] RegisterDTO user)
        {
            bool credenciaisValidas = false;
            if (user != null && !string.IsNullOrWhiteSpace(user.UserEmail))
            {
                // Verifica a existência do usuário nas tabelas do
                // ASP.NET Core Identity
                var userIdentity = await _userManager
                    .FindByNameAsync(user.UserEmail);
                if (userIdentity != null)
                {
                    // Efetua o login com base no Id do usuário e sua senha
                    var resultadoLogin = await _signInManager
                        .CheckPasswordSignInAsync(userIdentity, user.Password, false);
                    if (resultadoLogin.Succeeded)
                    {
                        // Since we want him to access all the app, 
                        //we don't care about roles 
                        // credenciaisValidas = await _userManager.IsInRoleAsync(
                        //     userIdentity, ApplicationRoles.ADMINROLE);
                    }
                }
            }

            return Ok();
        }
    }
}