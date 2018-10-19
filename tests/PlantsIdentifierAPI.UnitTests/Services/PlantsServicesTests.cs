﻿using AutoMapper;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using Moq;
using PlantsIdentifierAPI.Interfaces;
using PlantsIdentifierAPI.Models;
using PlantsIdentifierAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PlantsIdentifierAPI.UnitTests.Services
{
    public class PlantsServicesTests
    {

        public DbContextOptions<PlantsContext> DefaultOptions { get; } = new DbContextOptionsBuilder<PlantsContext>().Options;

        [Fact]
        public void Plants_GetAll_ReturnsOneElement()
        {
            //Arrange
            var dbContextMock = new DbContextMock<PlantsContext>(DefaultOptions);
            var mapperMock = new Mock<IMapper>();
            var plantsInitialDBSet = dbContextMock.CreateDbSetMock(x => x.Plant, new[] { new Plant { } });
            var service = new PlantsServices(dbContextMock.Object, mapperMock.Object);

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
            var dbContextMock = new DbContextMock<PlantsContext>(DefaultOptions);
            var mapperMock = new Mock<IMapper>();
            var plantsInitialDBSet = dbContextMock.CreateDbSetMock(x => x.Plant);
            var service = new PlantsServices(dbContextMock.Object, mapperMock.Object);

            //Act
            var result = service.GetAll();

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void Plants_GetPlantByID_ReturnsNull()
        {

            //Arrange
            //var dbContextMock = new DbContextMock<PlantsContext>(DefaultOptions);
            //var plantsInitialDBSet = dbContextMock.CreateDbSetMock(x => x.Plant);
            //var service = new PlantsServices(dbContextMock.Object);

            ////Act
            //var result = service.GetAll();

            ////Assert
            //Assert.NotNull(result);
            //Assert.Empty(result);
        }

        [Fact]
        public void Plants_GetPlantByID_ReturnsOk()
        {

        }

        [Fact]
        public void Plants_GetPlantByCommonName_ReturnsNull()
        {
        }

        [Fact]
        public void Plants_GetPlantByCommonName_ReturnsOk()
        {
        }
        
        [Fact]
        public void Plants_SavePlant_ReturnsNull()
        { }

        [Fact]
        public void Plants_SavePlant_ReturnsOk()
        { }



    }
}
