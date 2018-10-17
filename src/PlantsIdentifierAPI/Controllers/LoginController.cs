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
using PlantsIdentifierAPI.Interfaces;

namespace PlantsIdentifierAPI.Controllers
{
	[AllowAnonymous]
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		readonly TokenConfigurations _tokenConfigurations;
		readonly ILoginService _loginService;

		public LoginController(
					[FromServices]TokenConfigurations tokenConfigurations,
					[FromServices]ILoginService loginService)
		{
			_tokenConfigurations = tokenConfigurations;
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
		public async Task<IActionResult> Login([FromBody] RegisterDTO user)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var userIdentity = await _loginService.ValidateUser(user);
			if (userIdentity == null)
				return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized, Constants.WRONGEMAILORPASSWORD);
			else
				//This should be a method in Service and we should return the result of the method.
				return GenerateToken(userIdentity);
		}

		[HttpPost]
		public async Task<IActionResult> Refresh(string token, string refreshToken)
		{
			var user = await _loginService.GetUserFromToken(token);
			if (user.RefreshToken != refreshToken)
				throw new SecurityTokenException("Invalid refresh token");

			var newJwtToken = GenerateToken(user);
			var newRefreshToken = GenerateRefreshToken();

			//TODO We need to make this method more generic so that we only need to check this one method for a point of failure.
			_loginService.ReplaceRefreshToken(user, newRefreshToken);
			return new ObjectResult(new
			{
				token = newJwtToken,
				refreshToken = newRefreshToken
			});
		}
		

		IActionResult GenerateToken(ApplicationUser userIdentity)
		{
			var identity = new ClaimsIdentity(
					new GenericIdentity(userIdentity.UserName, "Login"),
					new[] {
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
						new Claim(JwtRegisteredClaimNames.UniqueName, userIdentity.UserName),
						new Claim("Email", userIdentity.Email)
					}
				);

			var createDate = DateTime.Now;
			var expiryDate = createDate + TimeSpan.FromSeconds(_tokenConfigurations.Seconds);

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

		

	}
}