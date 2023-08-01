using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
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
    public class UpdateCarShould
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
        public async Task UpdateAsync_CarCanBeEdited_ReturnsUpdatedCar()
        {
            // Arrange
            var car = TestHelpers.TestHelper.GetTestCarOne();
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var carToUpdate = TestHelpers.TestHelper.GetTestCarTwo();
            
            userManagerMock.Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Driver", "Administrator" });

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetByIdAsync(car.Id))
                .ReturnsAsync(car);
            carRepositoryMock.Setup(repo => repo.UpdateAsync(car.Id, It.IsAny<Car>()))
                .ReturnsAsync(carToUpdate);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = await carService.UpdateAsync(car.Id, carToUpdate, user);

            // Assert
            Assert.AreEqual(carToUpdate.Registration, result.Registration);
            Assert.AreEqual(carToUpdate.Color, result.Color);
            Assert.AreEqual(carToUpdate.TotalSeats, result.TotalSeats);
            Assert.AreEqual(carToUpdate.AvailableSeats, result.AvailableSeats);
            Assert.AreEqual(carToUpdate.Brand, result.Brand);
        }

        [TestMethod]
        public async Task UpdateAsync_CarCannotBeEdited_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var car = TestHelpers.TestHelper.GetTestCarOne();
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var carToUpdate = TestHelpers.TestHelper.GetTestCarTwo();

            userManagerMock.Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.GetByIdAsync(car.Id)).ReturnsAsync(carToUpdate);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act and Assert
            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await carService.UpdateAsync(car.Id, carToUpdate, user);
            });
        }
    }
}
