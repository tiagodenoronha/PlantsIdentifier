using EntityFrameworkCoreMock;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PlantsIdentifierAPI.Data;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Models;
using PlantsIdentifierAPI.Services;
using PlantsIdentifierAPI.UnitTests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Services
{
	public class LoginServicesTests
	{
		readonly UserManager<ApplicationUser> _userManagerMock;
		readonly SignInManager<ApplicationUser> _signinManagerMock;
		readonly SigningConfigurations _signingConfigurationsMock;
		readonly Mock<TokenConfigurations> _tokenConfigurationsMock;

		public LoginServicesTests()
		{
			_userManagerMock = new FakeUserManager();
			_signinManagerMock = new FakeSignInManager();
			var configurationMock = Mock.Of<IConfiguration>(conf => conf.GetValue<string>("TokenSecret") == It.IsAny<string>());
			_signingConfigurationsMock = new SigningConfigurations(configurationMock);
			_tokenConfigurationsMock = new Mock<TokenConfigurations>();
		}

		//[Fact]
		//public void Login_ValidateUser_ReturnsOk()
		//{
		//	//Arrange
		//	var mockEmail = "email";
		//	var mockUser = Mock.Of<ApplicationUser>(u => u.Email == mockEmail);
		//	var service = new LoginService(_userManagerMock, _signinManagerMock,
		//		_signingConfigurationsMock, _tokenConfigurationsMock.Object);

		//	//Act
		//	var result = service.ValidateUser(Mock.Of<LoginDTO>());

		//	//Assert
		//	Assert.NotNull(result);
		//}

	}
}
