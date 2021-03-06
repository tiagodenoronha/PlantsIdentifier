﻿using EntityFrameworkCoreMock;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using PlantsIdentifierAPI.Data;

namespace PlantsIdentifierAPI.UnitTests.Data
{
	public class IdentityInitializerTests
	{
		readonly DbContextMock<ApplicationDBContext> _dbContextMock;
		readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
		readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
		readonly Mock<IConfigurationRoot> _configurationRoot;
		readonly IdentityInitializer _identityInitializer;
		DbContextOptions<ApplicationDBContext> DefaultOptions { get; } = new DbContextOptionsBuilder<ApplicationDBContext>().Options;

		public IdentityInitializerTests()
		{
			_dbContextMock = new DbContextMock<ApplicationDBContext>(DefaultOptions);
			_userManagerMock = MockHelpers.MockUserManager<ApplicationUser>();
			_roleManagerMock = MockHelpers.MockRoleManager<IdentityRole>();
			_configurationRoot = new Mock<IConfigurationRoot>();
			_identityInitializer = new IdentityInitializer(_dbContextMock.Object, _userManagerMock.Object,
			_roleManagerMock.Object, _configurationRoot.Object);
		}
	}
}
