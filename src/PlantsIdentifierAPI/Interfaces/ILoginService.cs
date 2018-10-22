using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Models;

namespace PlantsIdentifierAPI.Interfaces
{
	public interface ILoginService
	{
		Task<ApplicationUser> ValidateUser(LoginDTO user);
		void ReplaceRefreshToken(ApplicationUser user, string newRefreshToken);
		Task<ApplicationUser> GetUserFromToken(string token);
		TokenModel GenerateToken(ApplicationUser userIdentity);
		string GenerateRefreshToken();
		Task<bool> UserExists(string email);
		Task<IdentityResult> CreateUser(string username, string email, string password);
	}
}
