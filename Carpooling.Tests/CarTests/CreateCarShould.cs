using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
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
    public class CreateCarShould
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
        public async Task CreateAsync_UserIsNotBlockedWithDriverRole_ReturnsCreatedCar()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var car = TestHelpers.TestHelper.GetTestCarOne();

            userManagerMock.Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Driver" });

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.CreateAsync(car))
                .ReturnsAsync(car);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);
            
            // Act
            var result = await carService.CreateAsync(car, user);

            // Assert
            Assert.AreEqual(car, result);
            Assert.AreEqual(user, car.Driver);
        }
        [TestMethod]
        public async Task CreateAsync_UserIsNotBlockedWithAdminRole_ReturnsCreatedCar()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var car = TestHelpers.TestHelper.GetTestCarOne();

            userManagerMock.Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string> { "Administrator" });

            var carRepositoryMock = new Mock<ICarRepository>();
            carRepositoryMock.Setup(repo => repo.CreateAsync(car))
                .ReturnsAsync(car);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = await carService.CreateAsync(car, user);

            // Assert
            Assert.AreEqual(car, result);
            Assert.AreEqual(user, car.Driver);
        }
        [TestMethod]
        public async Task CreateAsync_UserIsBlockedWithoutDriverOrAdminRole_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserFourBlocked();
            var car = TestHelpers.TestHelper.GetTestCarOne();

            userManagerMock.Setup(um => um.GetRolesAsync(user))
                .ReturnsAsync(new List<string>());

            var carRepositoryMock = new Mock<ICarRepository>();
            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await carService.CreateAsync(car, user);
            });
        }
    }
}
