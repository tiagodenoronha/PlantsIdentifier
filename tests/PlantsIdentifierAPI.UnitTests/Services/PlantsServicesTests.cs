using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using PlantsIdentifierAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Services
{
	public class PlantsServicesTests
	{

		//public DbContextOptions DefaultOptions { get; } = new DbContextOptionsBuilder().Options;

		//[Fact]
		//public void Plants_GetAll_ReturnsOk()
		//{
		//	var dbContextMock = new DbContextMock<PlantsContext>(DefaultOptions);
		//	//var usersDbSetMock = dbContextMock.CreateDbSetMock(x => x.Plant, initialEntities);

		//	//Arrange
		//	var mockContext = new Mock<PlantsContext>();
		//	var service = new PlantsServices(mockContext.Object);

		//	//Act
		//	var result = service.GetAll();

		//	//Assert
		//	Assert.Empty(result);
		//}

		//[Fact]
		//public void Plants_GetAll_ReturnsNull()
		//{
		//	var dbContextMock = new DbContextMock<PlantsContext>(DefaultOptions);

		//	//Arrange
		//	var service = new PlantsServices(dbContextMock.Object);

		//	//Act
		//	var result = service.GetAll();

		//	//Assert
		//	Assert.Empty(result);
		//}

	}
}
