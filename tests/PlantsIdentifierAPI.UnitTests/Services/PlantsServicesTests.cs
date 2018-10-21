﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using PlantsIdentifierAPI.Helpers;
using PlantsIdentifierAPI.Models;
using PlantsIdentifierAPI.Services;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Services
{
    public class PlantsServicesTests
    {
        readonly DbContextMock<PlantsContext> _dbContextMock;
        readonly IMapper _mapper;

        public PlantsServicesTests()
        {
            _dbContextMock = new DbContextMock<PlantsContext>(DefaultOptions);
            _mapper = new MapperConfiguration(c => c.AddProfile<AutoMapperProfile>()).CreateMapper();
        }

        public DbContextOptions<PlantsContext> DefaultOptions { get; } = new DbContextOptionsBuilder<PlantsContext>().Options;

        [Fact]
        public void Plants_GetAll_ReturnsOneElement()
        {
            //Arrange
            var plantsInitialDBSet = _dbContextMock.CreateDbSetMock(x => x.Plant, new[] { new Plant { } });
            var service = new PlantsServices(_dbContextMock.Object, _mapper);

            //Act
            var result = service.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Any());
        }

        [Fact]
        public void Plants_GetAll_ReturnsEmpty()
        {
            //Arrange
            var plantsInitialDBSet = _dbContextMock.CreateDbSetMock(x => x.Plant);
            var service = new PlantsServices(_dbContextMock.Object, _mapper);

            //Act
            var result = service.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Plants_GetPlantByID_ReturnsNull()
        {
            //Arrange
            var plantsInitialDBSet = _dbContextMock.CreateDbSetMock(x => x.Plant);
            var service = new PlantsServices(_dbContextMock.Object, _mapper);

            //Act
            var result = await service.GetPlant(Moq.It.IsAny<Guid>());

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Plants_GetPlantByID_ReturnsOk()
        {
            //Arrange
            var plantID = Guid.NewGuid();
            var plantsInitialDBSet = _dbContextMock.CreateDbSetMock(x => x.Plant, new[] { new Plant { ID = plantID } });
            var service = new PlantsServices(_dbContextMock.Object, _mapper);

            //Act
            var result = await service.GetPlant(plantID);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(result.ID, plantID);
        }

        [Fact]
        public async Task Plants_GetPlantByCommonName_ReturnsNull()
        {
            //Arrange
            var plantsInitialDBSet = _dbContextMock.CreateDbSetMock(x => x.Plant);
            var service = new PlantsServices(_dbContextMock.Object, _mapper);

            //Act
            var result = await service.GetPlantByCommonName(Moq.It.IsAny<string>());

            //Assert
            Assert.Null(result);
        }

        //[Fact]
        //public async Task Plants_GetPlantByCommonName_ReturnsOk()
        //{
        //    //Arrange
        //    var plantName = "Plant";
        //    var plantsInitialDBSet = _dbContextMock.CreateDbSetMock(x => x.Plant, new[] { new Plant { CommonName = plantName } });
        //    var service = new PlantsServices(_dbContextMock.Object, _mapperMock.Object);

        //    //Act
        //    var result = await service.GetPlantByCommonName(plantName);

        //    //Assert
        //    Assert.NotNull(result);
        //    Assert.Equal(result.CommonName, plantName);
        //}

        //[Fact]
        //public void Plants_SavePlant_ReturnsNull()
        //{
        //    //Arrange
        //    var plantsInitialDBSet = _dbContextMock.CreateDbSetMock(x => x.Plant);
        //    var service = new PlantsServices(_dbContextMock.Object, _mapperMock.Object);

        //    //Act
        //    service.SavePlant(Mock.Of<PlantDTO>());

        //    //Assert
        //    Assert.NotEmpty(_dbContextMock.Object.Plant);
        //}

        [Fact]
        public void Plants_SavePlant_ReturnsOk()
        { }



    }
}
