using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.CarTests
{
    [TestClass]
    public class GetCarsShould
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
        public async Task GetAllAsync_CarsExist_ReturnsListOfCars()
        {
            // Arrange
            var cars = TestHelpers.TestHelper.GetTestCars();

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(cars);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = await carService.GetAllAsync();

            // Assert
            Assert.AreEqual(cars.Count, result.Count);
            CollectionAssert.AreEquivalent(cars, result);
        }
        [TestMethod]
        public async Task GetAllAsync_NoCarsExist_ThrowsEmptyListException()
        {
            // Arrange

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new EmptyListException("No cars yet!"));

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<EmptyListException>(async () =>
            {
                await carService.GetAllAsync();
            });
        }
    }
}
