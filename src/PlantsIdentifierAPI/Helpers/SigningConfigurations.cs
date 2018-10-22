using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace PlantsIdentifierAPI.Helpers
{
	public class SigningConfigurations
	{
		public SecurityKey Key { get; }
		public SigningCredentials SigningCredentials { get; }

		public SigningConfigurations()
		{

		}

		public SigningConfigurations(IConfiguration configuration)
		{
			Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("TokenSecret")));

			SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
		}
	}
}