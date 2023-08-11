using AutoMapper;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Moq;
using Carpooling.BusinessLayer.Services;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;

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
        private Mock<IMapService> mapServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            travelRepositoryMock = new Mock<ITravelRepository>();
            mapperMock = new Mock<IMapper>();
            addressRepositoryMock = new Mock<IAddressRepository>();
            carRepositoryMock = new Mock<ICarRepository>();
            travelValidatorMock = new Mock<ITravelValidator>();
            userValidationMock = new Mock<IUserValidation>();
            mapServiceMock = new Mock<IMapService>();
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
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

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
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

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
                AvailableSeats = 4,
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
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

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

        [TestMethod]
        public async Task CreateTravelForMVCAsync_BannedUser_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserFourBlocked();
            var travel = new Travel();

            travelValidatorMock.Setup(validator => validator.ValidateIsLoggedUserAreDriver(loggedUser))
                .ThrowsAsync(new UnauthorizedOperationException("User is not allowed to create travel"));

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await travelService.CreateTravelForMVCAsync(loggedUser, travel);
            });
        }

        [TestMethod]
        public async Task CreateTravelForMVCAsync_ValidData_CreatesTravel()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var travel = new Travel
            {
                DriverId = "1",
                Car = TestHelpers.TestHelper.GetTestCarOne(),
                StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                DepartureTime = DateTime.Now.AddDays(1),
                ArrivalTime = DateTime.Now.AddDays(2)
            };

            travelValidatorMock.Setup(validator => validator.ValidateIsLoggedUserAreDriver(loggedUser))
                .ReturnsAsync(true);

            travelRepositoryMock.Setup(repo => repo.CreateTravelAsync(travel))
                .ReturnsAsync(travel);
            travelValidatorMock.Setup(validator => validator.ValidateIsNewTravelPossible(loggedUser.Id, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(true);
            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

            // Act
            var result = await travelService.CreateTravelForMVCAsync(loggedUser, travel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(travel.DriverId, result.DriverId);
            Assert.AreEqual(travel.Car.Registration, result.Car.Registration);

            travelRepositoryMock.Verify(repo => repo.CreateTravelAsync(travel), Times.Once);
        }


        [TestMethod]
        public async Task CreateTravelForMVCAsync_UnauthorizedCar_ThrowsEntityUnauthorizatedException()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserTwo();
            var travel = new Travel
            {
                DriverId = "1",
                Car = TestHelpers.TestHelper.GetTestCarOne(),
                StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                DepartureTime = DateTime.Now.AddDays(1),
                ArrivalTime = DateTime.Now.AddDays(2)
            };

            travelValidatorMock.Setup(validator => validator.ValidateIsLoggedUserAreDriver(loggedUser))
                .ReturnsAsync(true);

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<EntityUnauthorizatedException>(async () =>
            {
                await travelService.CreateTravelForMVCAsync(loggedUser, travel);
            });
        }
    }

}

