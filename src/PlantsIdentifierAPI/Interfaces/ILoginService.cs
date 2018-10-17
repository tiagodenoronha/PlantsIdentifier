using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.Models;

namespace PlantsIdentifierAPI.Interfaces
{
	public interface ILoginService
	{
		Task<ApplicationUser> ValidateUser(RegisterDTO user);
		void ReplaceRefreshToken(ApplicationUser user, string newRefreshToken);
		Task<ApplicationUser> GetUserFromToken(string token);
		TokenModel GenerateToken(ApplicationUser userIdentity);
		string GenerateRefreshToken();
	}
}
