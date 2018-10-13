using Moq;
using PlantsIdentifierAPI.Controllers;
using PlantsIdentifierAPI.Models;
using System;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Plants_Returns()
        {
            var mockRepo = new Mock<PlantsContext>();
           // mockRepo.Setup(repo => repo.Plant).ReturnsAsync(FillPlants());
            var controller = new PlantsController(mockRepo.Object);

            var result = controller.Get();


        }
    }
}
