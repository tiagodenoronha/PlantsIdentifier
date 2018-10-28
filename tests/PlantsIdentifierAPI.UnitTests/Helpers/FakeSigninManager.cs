using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PlantsIdentifierAPI.Data;

namespace PlantsIdentifierAPI.UnitTests.Helpers
{
	public class FakeSignInManager : SignInManager<ApplicationUser>
	{
		public FakeSignInManager(Mock<UserManager<ApplicationUser>> usermanager)
				: base(usermanager.Object,
					 new Mock<IHttpContextAccessor>().Object,
					 new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
					 new Mock<IOptions<IdentityOptions>>().Object,
					 new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
					 new Mock<IAuthenticationSchemeProvider>().Object)
		{ }
	}
}
