using Microsoft.AspNetCore.Mvc;
using Moq;
using PlantsIdentifierAPI.Controllers;
using PlantsIdentifierAPI.DTOS;
using PlantsIdentifierAPI.Interfaces;
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
		readonly PlantsController _controller;

		public PlantsControllerTests()
		{
			_mockRepo = new Mock<IPlantsServices>();
			_controller = new PlantsController(_mockRepo.Object);
		}

		[Fact]
		public void Plants_GetAll_ReturnsEmpty()
		{
			//Act
			var result = _controller.Get();

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

			//Act
			var result = _controller.Get();
			var contentResult = result.Result as OkObjectResult;
			var plants = (IEnumerable<PlantDTO>)contentResult.Value;

			//Assert
			Assert.IsType<ActionResult<IEnumerable<PlantDTO>>>(result);
			Assert.NotNull(contentResult.Value);
			Assert.NotEmpty(plants);
		}

		[Fact]
		public void Plants_GetAll_ReturnsInternalError()
		{
			//Arrange
			var exceptionMessage = "Message";
			_mockRepo.Setup(repo => repo.GetAll()).Throws(Mock.Of<Exception>(ex => ex.Message == exceptionMessage));

			//Act
			var result = _controller.Get();
			var contentResult = result.Result as ObjectResult;

			//Assert
			Assert.IsType<ActionResult<IEnumerable<PlantDTO>>>(result);
			Assert.Equal(500, contentResult.StatusCode);
			Assert.Equal(exceptionMessage, contentResult.Value);
		}

		[Fact]
		public async Task Plants_GetOneFromID_ReturnsOk()
		{
			//Arrange
			var guidToSearchFor = Guid.NewGuid();
			_mockRepo.Setup(repo => repo.GetPlant(It.IsAny<Guid>())).ReturnsAsync(Mock.Of<PlantDTO>(p => p.ID == guidToSearchFor));

			//Act
			var result = await _controller.Get(guidToSearchFor);

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
			//Act
			var result = await _controller.Get(Guid.NewGuid());
			var contentResult = result.Result as NotFoundObjectResult;

			//Assert
			Assert.IsType<ActionResult<PlantDTO>>(result);
			Assert.NotNull(contentResult);
			Assert.Equal(404, contentResult.StatusCode);
		}

		[Fact]
		public async Task Plants_GetOneFromID_ReturnsInternalError()
		{
			//Arrange
			var exceptionMessage = "Message";
			_mockRepo.Setup(repo => repo.GetPlant(It.IsAny<Guid>())).Throws(Mock.Of<Exception>(ex => ex.Message == exceptionMessage));

			//Act
			var result = await _controller.Get(It.IsAny<Guid>());
			var contentResult = result.Result as ObjectResult;

			//Assert
			Assert.IsType<ActionResult<PlantDTO>>(result);
			Assert.Equal(500, contentResult.StatusCode);
			Assert.Equal(exceptionMessage, contentResult.Value);
		}

		[Fact]
		public async Task Plants_Insert_ReturnsOk()
		{
			//Act
			var result = await _controller.Post(Mock.Of<PlantDTO>());
			var contentResult = result.Result as OkObjectResult;
			var inserted = (bool)contentResult.Value;

			//Assert
			Assert.IsType<ActionResult<bool>>(result);
			Assert.True(inserted);
		}

		[Fact]
		public async Task Plants_Insert_ReturnsConflict()
		{
			//Arrange
			var commonName = "Plant";
			var plant = Mock.Of<PlantDTO>();
			_mockRepo.Setup(repo => repo.GetPlantByCommonName(commonName)).ReturnsAsync(plant);

			//Act
			var result = await _controller.Post(Mock.Of<PlantDTO>(p => p.CommonName == commonName));
			var contentResult = result.Result as ConflictObjectResult;

			//Assert
			Assert.IsType<ActionResult<bool>>(result);
		}

		[Fact]
		public async Task Plants_Insert_ReturnsInternalError()
		{
			//Arrange
			var exceptionMessage = "Message";
			_mockRepo.Setup(repo => repo.GetPlantByCommonName(It.IsAny<string>())).Throws(Mock.Of<Exception>(ex => ex.Message == exceptionMessage));

			//Act
			var result = await _controller.Post(Mock.Of<PlantDTO>());
			var contentResult = result.Result as ObjectResult;

			//Assert
			Assert.IsType<ActionResult<bool>>(result);
			Assert.Equal(500, contentResult.StatusCode);
			Assert.Equal(exceptionMessage, contentResult.Value);
		}

		[Fact]
		public async Task Plants_Insert_ReturnsBadModel()
		{
			//Arrange
			_controller.ModelState.AddModelError("Error", "Error");

			//Act
			var result = await _controller.Post(Mock.Of<PlantDTO>());

			//Assert
			Assert.NotNull(result);
		}
	}
}
