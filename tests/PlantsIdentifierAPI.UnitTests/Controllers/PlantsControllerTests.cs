using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantsIdentifierAPI.Controllers;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Controllers
{
	public class PlantsControllerTests
	{
		readonly Mock<IPlantsServices> _mockRepo;

		public PlantsControllerTests()
		{
			_mockRepo = new Mock<IPlantsServices>();
		}

		[Fact]
		public void Plants_GetAll_ReturnsEmpty()
		{
			//Arrange
			var controller = new PlantsController(_mockRepo.Object);
			
			//Act
			var result = controller.Get();

			//Assert
			Assert.IsType<ActionResult<IEnumerable<PlantDTO>>>(result);
			Assert.Null(result.Value);
		}

		[Fact]
		public void Plants_GetAll_ReturnsOk()
		{
			//Arrange
			var numberOfPlants = 5;
			_mockRepo.Setup(repo => repo.GetAll()).Returns(Enumerable.Repeat(Mock.Of<PlantDTO>(), numberOfPlants));
			var controller = new PlantsController(_mockRepo.Object);

			//Act
			var result = controller.Get();
			var contentResult = result.Result as OkObjectResult;
			var plants = (IEnumerable<PlantDTO>)contentResult.Value;

			//Assert
			Assert.IsType<ActionResult<IEnumerable<PlantDTO>>>(result);
			Assert.NotNull(contentResult.Value);
			Assert.NotEmpty(plants);
		}

		[Fact]
		public async Task Plants_GetOneFromID_ReturnsOk()
		{
			//Arrange
			var guidToSearchFor = Guid.NewGuid();			
			_mockRepo.Setup(repo => repo.GetPlant(It.IsAny<Guid>())).ReturnsAsync(Mock.Of<PlantDTO>(p => p.ID == guidToSearchFor));
			var controller = new PlantsController(_mockRepo.Object);

			//Act
			var result = await controller.Get(guidToSearchFor);

			var contentResult = result.Result as OkObjectResult;
			var plant = (PlantDTO)contentResult.Value;

			//Assert
			Assert.IsType<ActionResult<PlantDTO>>(result);
			Assert.NotNull(contentResult.Value);
			Assert.Equal(guidToSearchFor, plant.ID);
		}

		[Fact]
		public async Task Plants_GetOneFromID_ReturnsEmpty()
		{
			//Arrange
			var controller = new PlantsController(_mockRepo.Object);

			//Act
			var result = await controller.Get(Guid.NewGuid());
			var contentResult = result.Result as NotFoundObjectResult;

			//Assert
			Assert.IsType<ActionResult<PlantDTO>>(result);
			Assert.NotNull(contentResult);
			Assert.Equal(404, contentResult.StatusCode);
		}

		[Fact]
		public async Task Plants_Insert_ReturnsOk()
		{
			//Arrange
			var controller = new PlantsController(_mockRepo.Object);

			//Act
			var result = await controller.Post(Mock.Of<PlantDTO>());
			var contentResult = result.Result as OkObjectResult;
			var inserted = (bool)contentResult.Value;

			//Assert
			Assert.IsType<ActionResult<bool>>(result);
			Assert.True(inserted);
		}

		[Fact]
		public async Task Plants_Insert_ReturnsConflict()
		{
			var commonName = "Plant";
			var plant = Mock.Of<PlantDTO>();

			//Arrange
			_mockRepo.Setup(repo => repo.GetPlantByCommonName(commonName)).ReturnsAsync(plant);
			var controller = new PlantsController(_mockRepo.Object);

			//Act
			var result = await controller.Post(Mock.Of<PlantDTO>(p => p.CommonName == commonName));
			var contentResult = result.Result as ConflictObjectResult;

			//Assert
			Assert.IsType<ActionResult<bool>>(result);			
		}

		[Fact]
		public async Task Plants_Insert_ReturnsBadModel()
		{
			var commonName = "Plant";
			var plant = Mock.Of<PlantDTO>(p => p.CommonName == string.Empty);

			//Arrange
			_mockRepo.Setup(repo => repo.GetPlantByCommonName(commonName)).ReturnsAsync(plant);
			var controller = new PlantsController(_mockRepo.Object);

			//Act
			var result = await controller.Post(Mock.Of<PlantDTO>(p => p.CommonName == commonName));
			var contentResult = result.Result as ConflictObjectResult;

			//Assert
			Assert.IsType<ActionResult<bool>>(result);
		}
	}
}
