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

		//[Fact]
		//public async Task Login_Login_ReturnsOk()
		//{
		//	//Arrange
		//	var useremail = "user";
		//	var mockDTO = Mock.Of<RegisterDTO>(u => u.UserEmail == useremail);
		//	var mockUser = Mock.Of<ApplicationUser>(appuser => appuser.Email == useremail);
		//	_loginService.Setup(service => service.ValidateUser(mockDTO)).Returns(Task.FromResult(mockUser));
		//	//_loginService.Setup(service => service.GenerateToken(mockUser)).Returns(Mock.Of<TokenModel>());
		//	var controller = new LoginController(_loginService.Object);

		//	//Act
		//	var result = await controller.Login(Mock.Of<RegisterDTO>()) as OkObjectResult;

		//	//Assert
		//	Assert.IsType<ActionResult<TokenModel>>(result);
		//	Assert.NotNull(result);
		//}
	}
}
