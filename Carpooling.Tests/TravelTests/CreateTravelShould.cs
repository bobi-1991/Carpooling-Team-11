using AutoMapper;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Repositories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpooling.BusinessLayer.Services;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.BusinessLayer.Exceptions;
using CarPooling.Data.Exceptions;

namespace Carpooling.Tests.TravelTests
{
    [TestClass]
    public class CreateTravelShould
    {
        private Mock<ITravelRepository> travelRepositoryMock;
        private Mock<IMapper> mapperMock;
        private Mock<IAddressRepository> addressRepositoryMock;
        private Mock<ICarRepository> carRepositoryMock;
        private Mock<ITravelValidator> travelValidatorMock;
        private Mock<IUserValidation> userValidationMock;

        [TestInitialize]
        public void Initialize()
        {
            travelRepositoryMock = new Mock<ITravelRepository>();
            mapperMock = new Mock<IMapper>();
            addressRepositoryMock = new Mock<IAddressRepository>();
            carRepositoryMock = new Mock<ICarRepository>();
            travelValidatorMock = new Mock<ITravelValidator>();
            userValidationMock = new Mock<IUserValidation>();
        }
        [TestMethod]
        public async Task CreateTravelAsync_BannedUser_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserFourBlocked();

            var travelRequest = new TravelRequest
            {
                DriverId = "1",
                DepartureTime = DateTime.Now,
                ArrivalTime = DateTime.Now,
                StartLocationId = 1,
                DestionationId = 2,
                AvailableSeats = 3,
                CarId = 4
            };
            var createdTravel = new Travel
            {
                Id = 1,
                Driver = TestHelpers.TestHelper.GetTestUserOne(),
                StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                Car = TestHelpers.TestHelper.GetTestCarTwo(),
                ArrivalTime = DateTime.Now,
                DepartureTime = DateTime.Now,
                IsCompleted = false
            };

            travelRepositoryMock.Setup(repo => repo.CreateTravelAsync(createdTravel)).ThrowsAsync(new UnauthorizedOperationException("You cannot create travel!"));

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object, 
                travelValidatorMock.Object, userValidationMock.Object);

            // Act & Assert

            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await travelService.CreateTravelAsync(loggedUser, travelRequest);
            });           
        }
        [TestMethod]
        public async Task CreateTravelAsync_InvalidTravelTime_ThrowsArgumentException()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var travelRequest = new TravelRequest
            {
                DriverId = "1",
                StartLocationId = 1,
                DestionationId = 2,
                CarId = 3,
                DepartureTime = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow.AddHours(2),
                AvailableSeats = 4
            };

            var startLocation = TestHelpers.TestHelper.GetTestAddressOne();
            var destination = TestHelpers.TestHelper.GetTestAddressTwo();
            var car = TestHelpers.TestHelper.GetTestCarOne();

            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(travelRequest.StartLocationId))
                .ReturnsAsync(startLocation);

            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(travelRequest.DestionationId))
                .ReturnsAsync(destination);

            carRepositoryMock.Setup(repo => repo.GetByIdAsync(travelRequest.CarId))
                .ReturnsAsync(car);

            travelValidatorMock.Setup(validator => validator.ValidateIsNewTravelPossible(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object);

            // Act & Assert
            
            Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await travelService.CreateTravelAsync(loggedUser, travelRequest);
            });
        }
        [TestMethod]
        public async Task CreateTravelAsync_ValidData_CreatesTravel()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var travelRequest = new TravelRequest
            {
                DriverId = "1",
                StartLocationId = 1,
                DestionationId = 2,
                CarId = 3,
                DepartureTime = new DateTime(2023, 10, 10),
                ArrivalTime = new DateTime(2023, 10, 12),
                AvailableSeats = 4
            };

            var startLocation = TestHelpers.TestHelper.GetTestAddressOne();
            var destination = TestHelpers.TestHelper.GetTestAddressTwo();
            var car = TestHelpers.TestHelper.GetTestCarTwo();
            var createdTravel = new Travel
            {
                Id = 1,
                Driver = TestHelpers.TestHelper.GetTestUserOne(),
                StartLocation = startLocation,
                EndLocation = destination,
                Car = car,
                DepartureTime = new DateTime(2023, 10, 10),
                ArrivalTime = new DateTime(2023, 10, 12),
                AvailableSeats =4,
                IsCompleted = false
            };

            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(travelRequest.StartLocationId))
                .ReturnsAsync(startLocation);
            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(travelRequest.DestionationId))
                .ReturnsAsync(destination);
            carRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(car);
            travelValidatorMock.Setup(validator => validator.ValidateIsNewTravelPossible(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);
            travelRepositoryMock.Setup(repo => repo.CreateTravelAsync(It.IsAny<Travel>()))
                .ReturnsAsync(createdTravel);

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object);

            // Act
            var result = await travelService.CreateTravelAsync(loggedUser, travelRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(startLocation.Details, result.StartLocationName);
            Assert.AreEqual(destination.Details, result.DestinationName);
            Assert.AreEqual(travelRequest.DepartureTime, result.DepartureTime);
            Assert.AreEqual(travelRequest.ArrivalTime, result.ArrivalTime);
            Assert.AreEqual(travelRequest.AvailableSeats, result.AvailableSeats);

            travelRepositoryMock.Verify(repo => repo.CreateTravelAsync(It.IsAny<Travel>()), Times.Once);
        }
    }
}
