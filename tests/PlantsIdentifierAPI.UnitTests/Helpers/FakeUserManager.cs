using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using PlantsIdentifierAPI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlantsIdentifierAPI.UnitTests.Helpers
{
	public class FakeUserManager : UserManager<ApplicationUser>
	{
		public FakeUserManager()
			: base(new Mock<IUserStore<ApplicationUser>>().Object,
			  new Mock<IOptions<IdentityOptions>>().Object,
			  new Mock<IPasswordHasher<ApplicationUser>>().Object,
			  new IUserValidator<ApplicationUser>[0],
			  new IPasswordValidator<ApplicationUser>[0],
			  new Mock<ILookupNormalizer>().Object,
			  new Mock<IdentityErrorDescriber>().Object,
			  new Mock<IServiceProvider>().Object,
			  new Mock<ILogger<UserManager<ApplicationUser>>>().Object)
		{ }

		public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password) => Task.FromResult(IdentityResult.Success);

		public override Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role) => Task.FromResult(IdentityResult.Success);

		public override Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user) => Task.FromResult(Guid.NewGuid().ToString());
	}
}
