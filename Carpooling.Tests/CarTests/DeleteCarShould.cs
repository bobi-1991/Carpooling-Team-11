using Carpooling.BusinessLayer.Exceptions;
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
    public class DeleteCarShould
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
        public async Task DeleteAsync_CarCanBeDeletedByOwner_ReturnsDeletedCar()
        {

            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var carToDelete = TestHelpers.TestHelper.GetTestCarOne();

            userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            var carRepositoryMock = new Mock<ICarRepository>();

            carRepositoryMock.Setup(repo => repo.GetByIdAsync(carToDelete.Id))
                .ReturnsAsync(carToDelete);

            carRepositoryMock.Setup(repo => repo.DeleteAsync(carToDelete.Id))
                .ReturnsAsync(carToDelete);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = await carService.DeleteAsync(carToDelete.Id, user);

            // Assert
            carRepositoryMock.Verify(repo => repo.DeleteAsync(carToDelete.Id), Times.Once);
        }
        [TestMethod]
        public async Task DeleteAsync_CarCanBeDeletedByAdmin_ReturnsDeletedCar()
        {

            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserTwo();
            var carToDelete = TestHelpers.TestHelper.GetTestCarOne();

            userManagerMock.Setup(um => um.GetRolesAsync(user))
               .ReturnsAsync(new List<string> { "Administrator"});

            var carRepositoryMock = new Mock<ICarRepository>();

            carRepositoryMock.Setup(repo => repo.GetByIdAsync(carToDelete.Id))
                .ReturnsAsync(carToDelete);

            carRepositoryMock.Setup(repo => repo.DeleteAsync(carToDelete.Id))
                .ReturnsAsync(carToDelete);

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act
            var result = await carService.DeleteAsync(carToDelete.Id, user);

            // Assert
            carRepositoryMock.Verify(repo => repo.DeleteAsync(carToDelete.Id), Times.Once);
        }
        [TestMethod]
        public async Task DeleteAsync_NoCarToBeDeleted_Throws()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserOne();

            userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string>());

            var carRepositoryMock = new Mock<ICarRepository>();

            carRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new EntityNotFoundException("No car to delete!"));

            var carService = new CarService(carRepositoryMock.Object, userManagerMock.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await carService.DeleteAsync(It.IsAny<int>(), user);
            });
        }
    }  
}
