using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.CarTests
{
    [TestClass]  
    public class GetCarShould
    {
        private Mock<UserManager<User>> userManagerMock;
        private Mock<IUserStore<User>> userStoreMock;
        [TestInitialize]

        public void Initialize()
        {

            userStoreMock = new Mock<IUserStore<User>>();
            userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
        [TestMethod]
        public async Task GetByIdAsync_CarExists_ReturnsCar()
        {
            // Arrange
            var car = TestHelpers.TestHelper.GetTestCarOne();

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(car);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = await carService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.AreEqual(car.Registration, result.Registration);
            Assert.AreEqual(car.Brand, result.Brand);
            Assert.AreEqual(car.Model, result.Model);
        }

        [TestMethod]
        public async Task GetByIdAsync_CarDoesNotExist_ThrowsEntityNotFoundException()
        {
            // Arrang

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new EntityNotFoundException("Car not found!"));

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await carService.GetByIdAsync(It.IsAny<int>());
            });
        }

        [TestMethod]
        public async Task GetByBrandModelAndRegistrationAsync_CarExists_ReturnsCar()
        {
            // Arrange
            var car = TestHelpers.TestHelper.GetTestCarOne();

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetByBrandModelAndRegistrationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(car);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = await carService.GetByBrandModelAndRegistrationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.AreEqual(car.Model, result.Model);
            Assert.AreEqual(car.Registration, result.Registration);
            Assert.AreEqual(car.Brand, result.Brand);
        }

        [TestMethod]
        public async Task GetByBrandModelAndRegistrationAsync_CarDoesNotExist_ReturnsNull()
        {
            // Arrange

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetByBrandModelAndRegistrationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new EntityNotFoundException("No such car!"));

            var carService = new CarService(carRepositoryMock.Object,userManagerMock.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
            await carService.GetByBrandModelAndRegistrationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            }); 
        }
    }
}
