using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.Services
{
	public class LoginService : ILoginService
	{
		readonly SigningConfigurations _signingConfigurations;
		readonly TokenConfigurations _tokenConfigurations;
		readonly UserManager<ApplicationUser> _userManager;
		readonly SignInManager<ApplicationUser> _signInManager;

		public LoginService([FromServices]UserManager<ApplicationUser> userManager, [FromServices]SignInManager<ApplicationUser> signInManager,
					[FromServices]SigningConfigurations signingConfigurations, [FromServices] TokenConfigurations tokenConfigurations)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_signingConfigurations = signingConfigurations;
			_tokenConfigurations = tokenConfigurations;
		}

		public async Task<ApplicationUser> GetUserFromToken(string token)
		{
			//TODO we need to check all the possible ways this breaks
			var principal = GetPrincipalFromExpiredToken(token);
			//Retrieving user from database
			return await _userManager.FindByNameAsync(principal.Identity.Name);
		}

		public void ReplaceRefreshToken(ApplicationUser user, string newRefreshToken)
		{
			//TODO we need to check if this returns an error or not with a try catch
			user.RefreshToken = newRefreshToken;
			_userManager.UpdateAsync(user);
		}

		public async Task<ApplicationUser> ValidateUser(LoginDTO user)
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

		public TokenModel GenerateToken(ApplicationUser userIdentity)
		{
			var identity = new ClaimsIdentity(
					new GenericIdentity(userIdentity.UserName, "Login"),
					new[] {
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
						new Claim(JwtRegisteredClaimNames.UniqueName, userIdentity.UserName)
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

			return new TokenModel
			{
				Authenticated = true,
				CreatedDate = createDate.ToString(Constants.DATEFORMAT),
				ExpirationDate = expiryDate.ToString(Constants.DATEFORMAT),
				AccessToken = token,
				RefreshToken = GenerateRefreshToken()
			};
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}

		public async Task<bool> UserExists(string email)
		{
			var userIdentity = await _userManager.FindByEmailAsync(email);
			return userIdentity != null;
		}

		public async Task<IdentityResult> CreateUser(string username, string email, string password)
		{
			var user = new ApplicationUser { UserName = username, Email = email };
			return await _userManager.CreateAsync(user, password);
		}
	}
}