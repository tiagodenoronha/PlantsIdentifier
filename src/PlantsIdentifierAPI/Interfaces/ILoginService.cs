using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Models;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.Interfaces
{
	public interface ILoginService
	{
		Task<ApplicationUser> ValidateUser(LoginDTO user);
		void ReplaceRefreshToken(ApplicationUser user, string newRefreshToken);
		Task<ApplicationUser> GetUserFromToken(string token);
		TokenModel GenerateAccessToken(ApplicationUser userIdentity);
		string GenerateRefreshToken();
		Task<bool> UserExists(string email);
		Task<TokenModel> CreateUser(string username, string email, string password);
	}
}
