using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.TravelTests
{
    [TestClass]
    public class UpdateTravelShould
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
        public async Task UpdateAsync_InvalidUpdateData_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var travelId = 123;
            var travelDataForUpdate = new TravelUpdateDto();

            var travelToUpdate = new Travel
            {
                Id = 1,
                Driver = TestHelpers.TestHelper.GetTestUserOne(),
                StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                Car = TestHelpers.TestHelper.GetTestCarTwo(),
                DepartureTime = new DateTime(2023, 10, 10),
                ArrivalTime = new DateTime(2023, 10, 12),
                AvailableSeats = 4,
                IsCompleted = false
            };

            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(travelId))
                .ReturnsAsync(travelToUpdate);

            userValidationMock.Setup(validator => validator.ValidateUserLoggedAndAdmin(loggedUser, travelToUpdate.DriverId))
                .ReturnsAsync(true);

            travelValidatorMock.Setup(validator => validator.CheckIsUpdateDataAreValid(travelToUpdate, travelDataForUpdate))
                .ReturnsAsync(false);
            travelRepositoryMock.Setup(repo => repo.UpdateAsync(travelId, travelToUpdate))
                .ThrowsAsync(new UnauthorizedOperationException("Please put correct input data for update."));
            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

            // Act & Assert
            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await travelService.UpdateAsync(loggedUser, It.IsAny<int>(), travelDataForUpdate);
            });
        }
        [TestMethod]
        public async Task UpdateAsync_BannedUser_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserFourBlocked();

            travelRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Travel>()))
                .ThrowsAsync(new UnauthorizedOperationException("You cannot update!"));
               

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object, 
                addressRepositoryMock.Object, carRepositoryMock.Object, travelValidatorMock.Object, 
                userValidationMock.Object, mapServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await travelService.UpdateAsync(loggedUser, It.IsAny<int>(), It.IsAny<TravelUpdateDto>());
            });
        }

        [TestMethod]
        public async Task UpdateAsync_ValidData_UpdatesTravel()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var travelId = 123;
            var travelDataForUpdate = new TravelUpdateDto
            {
                StartLocationId = 1,
                DestionationId = 2,
                CarId = 3,
                DepartureTime = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow.AddHours(2),
                AvailableSeats = 4
            };

            var travelToUpdate = new Travel
            {
                Driver = TestHelpers.TestHelper.GetTestUserOne(),
                StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                Car = TestHelpers.TestHelper.GetTestCarTwo(),
                DepartureTime = new DateTime(2023, 10, 10),
                ArrivalTime = new DateTime(2023, 10, 12),
                AvailableSeats = 4,
                IsCompleted = false
            };

            var startLocation = TestHelpers.TestHelper.GetTestAddressThree();
            var endLocation = TestHelpers.TestHelper.GetTestAddressFour();
            var updatedTravel = new Travel
            {
                Id = travelId,
                DriverId = "driverId",
                DepartureTime = DateTime.UtcNow,
                ArrivalTime = DateTime.UtcNow.AddHours(2),
                StartLocation = startLocation,
                EndLocation = endLocation,
                AvailableSeats = 4,
                Car = TestHelpers.TestHelper.GetTestCarOne(),
                IsCompleted = false
            };

            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(travelId))
                .ReturnsAsync(travelToUpdate);

            userValidationMock.Setup(validator => validator.ValidateUserLoggedAndAdmin(loggedUser, travelToUpdate.DriverId))
                .ReturnsAsync(true);

            travelValidatorMock.Setup(validator => validator.CheckIsUpdateDataAreValid(travelToUpdate, travelDataForUpdate))
                .ReturnsAsync(true);

            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(travelDataForUpdate.StartLocationId))
                .ReturnsAsync(startLocation);

            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(travelDataForUpdate.DestionationId))
                .ReturnsAsync(endLocation);

            carRepositoryMock.Setup(repo => repo.GetByIdAsync(travelDataForUpdate.CarId))
                .ReturnsAsync(TestHelpers.TestHelper.GetTestCarOne());

            travelRepositoryMock.Setup(repo => repo.UpdateAsync(travelId, It.IsAny<Travel>()))
                .ReturnsAsync(updatedTravel);

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

            // Act
            var result = await travelService.UpdateAsync(loggedUser, travelId, travelDataForUpdate);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(startLocation.City, result.StartLocationName);
            Assert.AreEqual(endLocation.City, result.DestinationName);
            Assert.AreEqual(travelDataForUpdate.AvailableSeats, result.AvailableSeats);

            travelRepositoryMock.Verify(repo => repo.UpdateAsync(travelId, It.IsAny<Travel>()), Times.Once);
        }
    }
}
