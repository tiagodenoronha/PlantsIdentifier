using Microsoft.AspNetCore.Mvc;
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
	}
}
