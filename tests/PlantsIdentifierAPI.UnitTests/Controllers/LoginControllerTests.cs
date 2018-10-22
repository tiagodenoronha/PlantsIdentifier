using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PlantsIdentifierAPI.Controllers;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Controllers
{
	public class LoginControllerTests
	{
		readonly Mock<ILoginService> _loginService;

		public LoginControllerTests()
		{
			_loginService = new Mock<ILoginService>();
		}

		[Fact]
		public async Task Login_Login_ReturnsOk()
		{
			//Arrange
			var mockUser = Mock.Of<ApplicationUser>();
			var mockToken = new TokenModel();

			//This fails, for some reason...
			//var mockToken = Mock.Of<TokenModel>();
			_loginService.Setup(service => service.ValidateUser(It.IsAny<LoginDTO>())).Returns(Task.FromResult(Mock.Of<ApplicationUser>()));
			_loginService.Setup(service => service.GenerateToken(It.IsAny<ApplicationUser>())).Returns(mockToken);
			var controller = new LoginController(_loginService.Object);

			//Act
			var result = await controller.Login(Mock.Of<LoginDTO>()) as OkObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsAssignableFrom<ActionResult>(result);
			Assert.NotNull(result.Value);
			Assert.IsType<TokenModel>(result.Value);
		}

		[Fact]
		public async Task Login_Login_ReturnsUnauthorized()
		{
			//Arrange
			var mockUser = Mock.Of<ApplicationUser>();
			_loginService.Setup(service => service.ValidateUser(It.IsAny<LoginDTO>())).Returns(Task.FromResult<ApplicationUser>(null));
			var controller = new LoginController(_loginService.Object);

			//Act
			var result = await controller.Login(Mock.Of<LoginDTO>());
			var contentResult = result as ObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsType<ObjectResult>(result);
			Assert.Equal(contentResult.Value, PlantsIdentifierAPI.Helpers.Constants.WRONGEMAILORPASSWORD);
		}

		[Fact]
		public async Task Login_Refresh_ReturnsUnauthorized()
		{
			//Arrange
			var mockRefreshToken = "mockrefreshtoken";
			var otherMockRefreshToken = "otherrefreshtoken";
			var mockUser = Mock.Of<ApplicationUser>(user => user.RefreshToken == otherMockRefreshToken);
			_loginService.Setup(service => service.GetUserFromToken(It.IsAny<string>())).Returns(Task.FromResult(mockUser));
			var controller = new LoginController(_loginService.Object);

			//Assert
			var exception = await Assert.ThrowsAsync<SecurityTokenException>(() => controller.Refresh(It.IsAny<string>(), mockRefreshToken));
			Assert.Equal(exception.Message, PlantsIdentifierAPI.Helpers.Constants.INVALIDREFRESHTOKEN);
		}

		[Fact]
		public async Task Login_Refresh_ReturnsOk()
		{
			//Arrange
			var mockRefreshToken = "mockrefreshtoken";
			var mockUser = Mock.Of<ApplicationUser>(user => user.RefreshToken == mockRefreshToken);
			_loginService.Setup(service => service.GetUserFromToken(It.IsAny<string>())).Returns(Task.FromResult(mockUser));
			var controller = new LoginController(_loginService.Object);

			//Act
			var result = await controller.Refresh(It.IsAny<string>(), mockRefreshToken) as ObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsAssignableFrom<ActionResult>(result);
			Assert.NotNull(result.Value);
		}
	}
}
