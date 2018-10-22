using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Microsoft.Extensions.Options;
using Moq;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Models;
using PlantsIdentifierAPI.Services;
using PlantsIdentifierAPI.UnitTests.Helpers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Services
{
	public class LoginServicesTests
	{
		readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
		readonly SignInManager<ApplicationUser> _signinManagerMock;
		readonly Mock<SigningConfigurations> _signingConfigurationsMock;
		readonly Mock<TokenConfigurations> _tokenConfigurationsMock;
		readonly Mock<IUserStore<ApplicationUser>> _userStoreMock;

		public LoginServicesTests()
		{
			_userManagerMock = MockHelpers.MockUserManager<ApplicationUser>();
			var context = new Mock<HttpContext>();
			var contextAccessor = new Mock<IHttpContextAccessor>();
			contextAccessor.Setup(a => a.HttpContext).Returns(context.Object);
			var roleManager = MockHelpers.MockRoleManager<PocoRole>();
			var identityOptions = new IdentityOptions();
			var options = new Mock<IOptions<IdentityOptions>>();
			options.Setup(a => a.Value).Returns(identityOptions);
			var claimsFactory = new UserClaimsPrincipalFactory<ApplicationUser, PocoRole>(_userManagerMock.Object, roleManager.Object, options.Object);
			var logStore = new StringBuilder();
			var logger = MockHelpers.MockILogger<SignInManager<ApplicationUser>>(logStore);
			_signinManagerMock = new SignInManager<ApplicationUser>(_userManagerMock.Object, contextAccessor.Object, claimsFactory, options.Object, logger.Object, new Mock<IAuthenticationSchemeProvider>().Object);
			_signingConfigurationsMock = new Mock<SigningConfigurations>();
			_tokenConfigurationsMock = new Mock<TokenConfigurations>();
		}

		//[Fact]
		//public async Task Login_ValidateUser_ReturnsOk()
		//{
		//	//Arrange
		//	var mockEmail = "email";
		//	var mockUser = Mock.Of<ApplicationUser>(u => u.Email == mockEmail);
		//	var mockLoginDTO = Mock.Of<LoginDTO>(user => user.UserEmail == mockEmail);

		//	var service = new LoginService(_userManagerMock.Object, _signinManagerMock,
		//		_signingConfigurationsMock.Object, _tokenConfigurationsMock.Object);
			
		//	//Act
		//	var result = await service.ValidateUser(mockLoginDTO);

		//	//Assert
		//	Assert.NotNull(result);
		//}
	}
}
