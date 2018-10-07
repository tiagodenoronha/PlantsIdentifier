using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PlantsIdentifierAPI.Helpers
{
    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ULTRASUPERAWESOMEPASSWORDWHICHCANNOTBECRACKED"));

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}