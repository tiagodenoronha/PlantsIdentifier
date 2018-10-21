using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.DTOS;

namespace PlantsIdentifierAPI.Controllers
{
	[AllowAnonymous]
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		readonly ILoginService _loginService;

		public LoginController([FromServices]ILoginService loginService)
		{
			_loginService = loginService;
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
		public async Task<IActionResult> Login([FromBody] LoginDTO user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var userIdentity = await _loginService.ValidateUser(user);
			if (userIdentity == null)
				return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized, Constants.WRONGEMAILORPASSWORD);
			else
			{
				var token = _loginService.GenerateToken(userIdentity);
				return Ok(token);
			}
		}

		[HttpPost]
		[ProducesResponseType(200)]
		public async Task<IActionResult> Refresh(string token, string refreshToken)
		{
			var user = await _loginService.GetUserFromToken(token);
			if (user.RefreshToken != refreshToken)
				throw new SecurityTokenException("Invalid refresh token");

			var newJwtToken = _loginService.GenerateToken(user);
			var newRefreshToken = _loginService.GenerateRefreshToken();

			//TODO We need to make this method more generic so that we only need to check this one method for a point of failure.
			_loginService.ReplaceRefreshToken(user, newRefreshToken);
			return new ObjectResult(new
			{
				token = newJwtToken,
				refreshToken = newRefreshToken
			});
		}
	}
}