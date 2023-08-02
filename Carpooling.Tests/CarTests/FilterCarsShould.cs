using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.Tests.TestHelpers;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.CarTests
{
    public class FilterCarsShould
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
        public void When_ParamsAreValidFilterByBrand_ShouldSucceed()
        {
            //Arrange
            List<Car> cars = TestHelper.GetTestCars();
            List<Car> expectedCars = TestHelper.GetTestCarsFilteredByBrand();
            User expectedUser = TestHelper.GetTestUserOne();
            var repositoryMock = new Mock<ICarRepository>();
            repositoryMock
                    .Setup(repo => repo.GetAllAsync())
                    .ReturnsAsync(cars);
            repositoryMock
                .Setup(repo => repo.FilterCarsAndSortAsync("brand"))
                .ReturnsAsync(expectedCars);


            var sut = new CarService(repositoryMock.Object, userManagerMock.Object);

            //Act
            var result = sut.FilterCarsAndSortAsync("brand");

            //Assert
            repositoryMock.Verify(repo => repo.FilterCarsAndSortAsync("brand"), Times.Once);

        }
        [TestMethod]
        public void When_ParamsAreValidFilterByModel_ShouldSucceed()
        {
            //Arrange
            List<Car> cars = TestHelper.GetTestCars();
            List<Car> expectedCars = TestHelper.GetTestCarsFilteredByBrand();
            var repositoryMock = new Mock<ICarRepository>();
            repositoryMock
                    .Setup(repo => repo.GetAllAsync())
                    .ReturnsAsync(cars);
            repositoryMock
                .Setup(repo => repo.FilterCarsAndSortAsync("model"))
                .ReturnsAsync(expectedCars);


            var sut = new CarService(repositoryMock.Object, userManagerMock.Object);

            //Act
            var result = sut.FilterCarsAndSortAsync("model");

            //Assert
            Assert.AreEqual(result, expectedCars);
            repositoryMock.Verify(repo => repo.FilterCarsAndSortAsync("model"), Times.Once);

        }
        [TestMethod]
        public void When_ParamsAreValidFilterByDate_ShouldSucceed()
        {
            //Arrange
            List<Car> cars = TestHelper.GetTestCars();
            List<Car> expectedCars = TestHelper.GetTestCarsFilteredByCreateTime();
            var repositoryMock = new Mock<ICarRepository>();
            repositoryMock
                    .Setup(repo => repo.GetAllAsync())
                    .ReturnsAsync(cars);
            repositoryMock
                .Setup(repo => repo.FilterCarsAndSortAsync("date"))
                .ReturnsAsync(expectedCars);


            var sut = new CarService(repositoryMock.Object, userManagerMock.Object);

            //Act
            var result = sut.FilterCarsAndSortAsync("date");

            //Assert
            Assert.AreEqual(result, expectedCars);
            repositoryMock.Verify(repo => repo.FilterCarsAndSortAsync("date"), Times.Once);

        }
        [TestMethod]
        public void When_ParamsAreValidFilterById_ShouldSucceed()
        {
            //Arrange
            List<Car> cars = TestHelper.GetTestCars();
            List<Car> expectedCars = TestHelper.GetTestCars();
            var repositoryMock = new Mock<ICarRepository>();
            repositoryMock
                    .Setup(repo => repo.GetAllAsync())
                    .ReturnsAsync(cars);
            repositoryMock
                .Setup(repo => repo.FilterCarsAndSortAsync("id"))
                .ReturnsAsync(expectedCars);


            var sut = new CarService(repositoryMock.Object, userManagerMock.Object);

            //Act
            var result = sut.FilterCarsAndSortAsync("id");

            //Assert
            Assert.AreEqual(result, expectedCars);
            repositoryMock.Verify(repo => repo.FilterCarsAndSortAsync("id"), Times.Once);
        }
        [TestMethod]
        public void When_ParamsAreValidButThereAreNoCars_ShouldThrow()
        {

            var repositoryMock = new Mock<ICarRepository>();
            repositoryMock
                    .Setup(repo => repo.GetAllAsync())
                    .ThrowsAsync(new EmptyListException("No cars yet!"));
            repositoryMock
                .Setup(repo => repo.FilterCarsAndSortAsync(It.IsAny<string>()))
                .ThrowsAsync(new EmptyListException("No cars yet!"));


            var sut = new CarService(repositoryMock.Object, userManagerMock.Object);

            //Act & Assert
            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await sut.FilterCarsAndSortAsync(It.IsAny<string>());
            });
        }
    }
}
