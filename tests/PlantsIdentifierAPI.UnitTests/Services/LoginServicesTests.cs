using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Microsoft.Extensions.Options;
using Moq;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Models;
using PlantsIdentifierAPI.Services;
using System;
using System.Linq;
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
		readonly TokenConfigurations _tokenConfigurationsMock;
		readonly LoginService _service;

		public LoginServicesTests()
		{
			_userManagerMock = MockHelpers.MockUserManager<ApplicationUser>();
			_signinManagerMock = new SignInManager<ApplicationUser>(_userManagerMock.Object, new Mock<IHttpContextAccessor>().Object,
				new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, new Mock<IOptions<IdentityOptions>>().Object,
				MockHelpers.MockILogger<SignInManager<ApplicationUser>>(new StringBuilder()).Object, new Mock<IAuthenticationSchemeProvider>().Object);
			_signingConfigurationsMock = new Mock<SigningConfigurations>();
			_tokenConfigurationsMock = Mock.Of<TokenConfigurations>();
			_service = new LoginService(_userManagerMock.Object, _signinManagerMock,
				_signingConfigurationsMock.Object, _tokenConfigurationsMock);
		}

		[Fact]
		public void Login_GenerateToken_ReturnsOk()
		{
			//Arrange
			var username = "Username";
			var mockUser = Mock.Of<ApplicationUser>(user => user.UserName == username);
			_tokenConfigurationsMock.Seconds = 1000;

			//Act
			var result = _service.GenerateAccessToken(mockUser);

			//Assert
			Assert.NotNull(result);
			Assert.IsType<TokenModel>(result);
			Assert.Equal(result.ExpirationDate, DateTime.Parse(result.CreatedDate).AddSeconds(_tokenConfigurationsMock.Seconds).ToString(Constants.DATEFORMAT));
			Assert.NotNull(result.RefreshToken);
		}

		[Fact]
		public void Login_GenerateToken_ReturnsError()
		{
			//Arrange
			var mockUser = Mock.Of<ApplicationUser>();

			//Assert
			var exception = Assert.Throws<ArgumentNullException>(() => _service.GenerateAccessToken(mockUser));
		}

		[Fact]
		public void Login_ReplaceRefreshToken_ReturnsOk()
		{
			//Arrange
			var mockRefreshToken = "RefreshToken";
			var otherMockRefreshToken = "RefreshToken2";
			var mockUser = Mock.Of<ApplicationUser>(user => user.RefreshToken == mockRefreshToken);
			_userManagerMock.Setup(manager => manager.UpdateAsync(mockUser)).ReturnsAsync(Mock.Of<IdentityResult>(ir => ir.Succeeded == true));
			_userManagerMock.Object.Users.Append(mockUser);

			//Act
			_service.ReplaceRefreshToken(mockUser, otherMockRefreshToken);

			//Assert
			//TODO what should we actually test for?
		}

		[Fact]
		public async Task Login_UserExists_ReturnsOk()
		{
			//Arrange
			_userManagerMock.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(Mock.Of<ApplicationUser>());

			//Act
			var result = await _service.UserExists(It.IsAny<string>());

			//Assert
			Assert.True(result);
		}

		[Fact]
		public async Task Login_UserExists_ReturnsFalse()
		{
			//Arrange
			_userManagerMock.Setup(manager => manager.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

			//Act
			var result = await _service.UserExists(It.IsAny<string>());

			//Assert
			Assert.False(result);
		}

		[Fact]
		public async Task Login_CreateUser_ReturnsOk()
		{
			//Arrange
			var mockUser = "user";
			var mockEmail = "email";
			var mockPassword = "password";
			_userManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(Mock.Of<IdentityResult>(ir => ir.Succeeded == true));
			_tokenConfigurationsMock.Seconds = 1000;

			//Act
			var result = await _service.CreateUser(mockUser, mockEmail, mockPassword);

			//Assert
			Assert.NotNull(result);
			Assert.NotNull(result.AccessToken);
			Assert.NotNull(result.RefreshToken);
			Assert.True(result.Authenticated);
		}

		[Fact]
		public async Task Login_CreateUser_ReturnsFalse()
		{
			//Arrange
			var mockUser = "user";
			var mockEmail = "email";
			var mockPassword = "password";
			_userManagerMock.Setup(manager => manager.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(Mock.Of<IdentityResult>(ir => ir.Succeeded == false));
			_tokenConfigurationsMock.Seconds = 1000;

			//Act
			var result = await _service.CreateUser(mockUser, mockEmail, mockPassword);

			//Assert
			Assert.Null(result);
		}
	}
}
