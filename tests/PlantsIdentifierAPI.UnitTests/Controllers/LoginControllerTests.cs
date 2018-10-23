using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using PlantsIdentifierAPI.Controllers;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Controllers
{
	public class LoginControllerTests
	{
		readonly Mock<ILoginService> _loginService;
		readonly LoginController _controller;

		public LoginControllerTests()
		{
			_loginService = new Mock<ILoginService>();
			_controller = new LoginController(_loginService.Object);
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

			//Act
			var result = await _controller.Login(Mock.Of<LoginDTO>()) as OkObjectResult;

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

			//Act
			var result = await _controller.Login(Mock.Of<LoginDTO>());
			var contentResult = result as ObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsType<ObjectResult>(result);
			Assert.Equal(contentResult.Value, PlantsIdentifierAPI.Helpers.Constants.WRONGEMAILORPASSWORD);
		}

		[Fact]
		public async Task Login_Login_ReturnsBadModel()
		{
			//Arrange
			_controller.ModelState.AddModelError("Error", "Error");

			//Act
			var result = await _controller.Login(Mock.Of<LoginDTO>()) as BadRequestObjectResult;
			//Assert
			Assert.NotNull(result);
			Assert.Equal(400, result.StatusCode);
		}

		[Fact]
		public async Task Login_Refresh_ReturnsOk()
		{
			//Arrange
			var mockRefreshToken = "mockrefreshtoken";
			var mockUser = Mock.Of<ApplicationUser>(user => user.RefreshToken == mockRefreshToken);
			_loginService.Setup(service => service.GetUserFromToken(It.IsAny<string>())).Returns(Task.FromResult(mockUser));

			//Act
			var result = await _controller.Refresh(It.IsAny<string>(), mockRefreshToken) as ObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsAssignableFrom<ActionResult>(result);
			Assert.NotNull(result.Value);
		}

		[Fact]
		public async Task Login_Refresh_ReturnsUnauthorized()
		{
			//Arrange
			var mockRefreshToken = "mockrefreshtoken";
			var otherMockRefreshToken = "otherrefreshtoken";
			var mockUser = Mock.Of<ApplicationUser>(user => user.RefreshToken == otherMockRefreshToken);
			_loginService.Setup(service => service.GetUserFromToken(It.IsAny<string>())).Returns(Task.FromResult(mockUser));

			//Act
			var result = await _controller.Refresh(It.IsAny<string>(), mockRefreshToken) as BadRequestObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.Equal(result.Value, PlantsIdentifierAPI.Helpers.Constants.INVALIDREFRESHTOKEN);
		}

		[Fact]
		public async Task Login_Register_ReturnsOk()
		{
			//Arrange
			_loginService.Setup(service => service.UserExists(It.IsAny<string>())).ReturnsAsync(false);
			_loginService.Setup(service => service.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(Mock.Of<IdentityResult>(ir => ir.Succeeded == true));

			//Act
			var result = await _controller.Register(Mock.Of<RegisterDTO>());
			var contentResult = result.Result as ObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsType<ActionResult<IdentityResult>>(result);
			Assert.NotNull(result.Value);
			Assert.True(result.Value.Succeeded);
		}

		[Fact]
		public async Task Login_Register_ReturnsUnauthorized()
		{
			//Arrange
			_loginService.Setup(service => service.UserExists(It.IsAny<string>())).ReturnsAsync(true);

			//Act
			var result = await _controller.Register(Mock.Of<RegisterDTO>());
			var contentResult = result.Result as ObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsType<ActionResult<IdentityResult>>(result);
			Assert.NotNull(contentResult);
			Assert.Equal(401, contentResult.StatusCode);
			Assert.Equal(PlantsIdentifierAPI.Helpers.Constants.USERALREADYEXISTS, contentResult.Value);
		}

		[Fact]
		public async Task Login_Register_ReturnsBadModel()
		{
			//Arrange
			_controller.ModelState.AddModelError("Error", "Error");

			//Act
			var result = await _controller.Register(Mock.Of<RegisterDTO>());
			var contentResult = result.Result as BadRequestObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsType<ActionResult<IdentityResult>>(result);
			Assert.NotNull(contentResult);
			Assert.Equal(400, contentResult.StatusCode);
		}

		[Fact]
		public async Task Login_Register_ReturnsBadRequest()
		{
			//Arrange
			var password = "Password";
			var otherPassword = "OtherPassword";
			var mockRegisterDTO = Mock.Of<RegisterDTO>(user => user.Password == password && user.ConfirmPassword == otherPassword);

			//Act
			var result = await _controller.Register(mockRegisterDTO);
			var contentResult = result.Result as BadRequestObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsType<ActionResult<IdentityResult>>(result);
			Assert.NotNull(contentResult);
			Assert.Equal(400, contentResult.StatusCode);
			Assert.Equal(PlantsIdentifierAPI.Helpers.Constants.PASSWORDMISMATCH, contentResult.Value);
		}

		[Fact]
		public async Task Login_Register_ReturnsNotOkDueToService()
		{
			//Arrange
			_loginService.Setup(service => service.UserExists(It.IsAny<string>())).ReturnsAsync(false);
			_loginService.Setup(service => service.CreateUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(Mock.Of<IdentityResult>(ir => ir.Succeeded == false));

			//Act
			var result = await _controller.Register(Mock.Of<RegisterDTO>());
			var contentResult = result.Result as ObjectResult;

			//Assert
			Assert.NotNull(result);
			Assert.IsType<ActionResult<IdentityResult>>(result);
			Assert.NotNull(result.Value);
			Assert.False(result.Value.Succeeded);
		}
	}
}
