using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantsIdentifierAPI.Controllers;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests
{
	public class UnitTest1
	{
		[Fact]
		public void Plants_ReturnsEmpty()
		{
			var mockRepo = new Mock<IPlantsServices>();
		   // mockRepo.Setup(repo => repo.Plant).ReturnsAsync(FillPlants());
			var controller = new PlantsController(mockRepo.Object);

			var result = controller.Get();
			Assert.IsType<ActionResult<IEnumerable<Plant>>>(result);
			Assert.Null(result.Value);

		}
		[Fact]
		public void Plants_ReturnsOk()
		{
			var numberOfPlants = 5;
			var mockRepo = new Mock<IPlantsServices>();
			mockRepo.Setup(repo => repo.GetAll()).Returns(Enumerable.Repeat(Mock.Of<Plant>(), numberOfPlants));
			var controller = new PlantsController(mockRepo.Object);

			var result = controller.Get();
			Assert.IsType<ActionResult<IEnumerable<Plant>>>(result);		

		}
	}
}
