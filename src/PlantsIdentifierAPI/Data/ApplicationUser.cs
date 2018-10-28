using Microsoft.AspNetCore.Identity;

namespace PlantsIdentifierAPI.Data
{
	public class ApplicationUser : IdentityUser
    {
        public string RefreshToken { get; set; }
    }
}