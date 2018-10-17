using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.Services
{
	public class LoginService : ILoginService
	{
		readonly SigningConfigurations _signingConfigurations;
		readonly UserManager<ApplicationUser> _userManager;
		readonly SignInManager<ApplicationUser> _signInManager;

		public LoginService([FromServices]UserManager<ApplicationUser> userManager, [FromServices]SignInManager<ApplicationUser> signInManager,
					[FromServices]SigningConfigurations signingConfigurations)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_signingConfigurations = signingConfigurations;
		}

		internal async Task<ApplicationUser> GetUserFromToken(string token)
		{
			//TODO we need to check all the possible ways this breaks
			var principal = GetPrincipalFromExpiredToken(token);
			//Retrieving user from database
			var user = await _userManager.FindByNameAsync(principal.Identity.Name);
		}

		internal void ReplaceRefreshToken(ApplicationUser user, string newRefreshToken)
		{
			//TODO we need to check if this returns an error or not with a try catch
			user.RefreshToken = newRefreshToken;
			_userManager.UpdateAsync(user);
		}

		internal async Task<ApplicationUser> ValidateUser(RegisterDTO user)
		{
			if (user != null && !string.IsNullOrWhiteSpace(user.UserEmail))
			{
				//Check if the user exists on the database
				var userIdentity = await _userManager.FindByEmailAsync(user.UserEmail);
				if (userIdentity != null)
				{
					// Login using the userID and password
					var loginResult = await _signInManager.CheckPasswordSignInAsync(userIdentity, user.Password, false);
					if (loginResult.Succeeded)
						return userIdentity;
				}
			}
			return null;
		}

		ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
			if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");
			return principal;
		}
	}
}