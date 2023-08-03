using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarPooling.Data.Models.Enums;
using Carpooling.BusinessLayer.Validation;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Exceptions;

namespace Carpooling.Tests.TripRequestTests
{
    [TestClass]
    public class CreateTripRequestShould
    {
        private Mock<ITripRequestRepository> tripRequestRepositoryMock;
        private Mock<IUserValidation> userValidationMock;
        private Mock<ITravelRepository> travelRepositoryMock;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<ITripRequestValidator> tripRequestValidatorMock;
        private TripRequestService tripRequestService;

        [TestInitialize]
        public void Initialize()
        {
            tripRequestRepositoryMock = new Mock<ITripRequestRepository>();
            userValidationMock = new Mock<IUserValidation>();
            travelRepositoryMock = new Mock<ITravelRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            tripRequestValidatorMock = new Mock<ITripRequestValidator>();

            tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                travelRepositoryMock.Object,
                userRepositoryMock.Object,
                tripRequestValidatorMock.Object);
        }
        [TestMethod]
        public async Task CreateAsync_ValidRequest_ReturnsCreatedTripRequest()
        {
            // Arrange
            var travelId = 1;
            var tripRequestRequest = new TripRequestRequest
            {
                PassengerId = "2",
                TravelId = travelId
            };

            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var passenger = TestHelpers.TestHelper.GetTestUserTwo() ;
            var travel = new Travel 
            {
                Id = 1,
                Driver = TestHelpers.TestHelper.GetTestUserOne(),
                DriverId = "1",
                StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                Car = TestHelpers.TestHelper.GetTestCarTwo(),
                ArrivalTime = DateTime.Now,
                DepartureTime = DateTime.Now,
                IsCompleted = false
            };
            var tripRequest = new TripRequest
            {
                Passenger = passenger,
                Driver= TestHelpers.TestHelper.GetTestUserOne(),
                DriverId= "1",
                Travel = travel,
                Status = TripRequestEnum.Pending
            };

            userRepositoryMock.Setup(repo => repo.GetByIdAsync(passenger.Id))
                .ReturnsAsync(passenger);

            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(travelId)).ReturnsAsync(travel);
            tripRequestRepositoryMock.Setup(repo => repo.CreateAsync(travel.DriverId, passenger.Id, travelId))
                .ReturnsAsync(tripRequest);

            tripRequestValidatorMock.Setup(validator => validator.ValidateIfPassengerAlreadyCreateTripRequest(tripRequestRequest))
                .ReturnsAsync(false);
            userValidationMock.Setup(validator => validator.ValidateUserNotBanned(loggedUser))
                .ReturnsAsync(true);
            tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object, 
                travelRepositoryMock.Object,
                userRepositoryMock.Object,
                tripRequestValidatorMock.Object);

            // Act
            var result = await tripRequestService.CreateAsync(loggedUser, tripRequestRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(passenger.UserName, result.PassengerUsername);
            Assert.AreEqual(travel.StartLocation.Details, result.StartLocationDetails);
            Assert.AreEqual(travel.EndLocation.Details, result.EndLocationDetails);
            Assert.AreEqual(travel.DepartureTime, result.DepartureTime);
            Assert.AreEqual(tripRequest.Status.ToString(), result.Status);
        }
       
        [TestMethod]
        public async Task CreateAsync_PassengerIsBlocked_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var travelId = 1;
            var tripRequestRequest = new TripRequestRequest
            {
                PassengerId = "2",
                TravelId = travelId
            };

            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var passenger = TestHelpers.TestHelper.GetTestUserFourBlocked() ;
            var travel = new Travel 
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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(passenger.Id))
                .ReturnsAsync(passenger);

            var travelRepositoryMock = new Mock<ITravelRepository>();
            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(travelId))
                .ReturnsAsync(travel);
            travelRepositoryMock.Setup(repo => repo.CreateTravelAsync(It.IsAny<Travel>()))
                .ThrowsAsync(new UnauthorizedOperationException("You cannot create travel"));

            var tripRequestValidatorMock = new Mock<ITripRequestValidator>();
            tripRequestValidatorMock.Setup(validator => validator.ValidateIfPassengerAlreadyCreateTripRequest(tripRequestRequest))
                .ReturnsAsync(false);
            var userValidationMock = new Mock<IUserValidation>();
            userValidationMock.Setup(validator => validator.ValidateUserNotBanned(loggedUser))
                .ReturnsAsync(false);

            var tripRequestService = new TripRequestService(
                null, 
                null,
                travelRepositoryMock.Object,
                userRepositoryMock.Object,
                null);

            // Act & Assert

            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await tripRequestService.CreateAsync(passenger, It.IsAny<TripRequestRequest>());
            });
        }
        [TestMethod]
        public async Task CreateAsync_DuplicateRequest_ThrowsDuplicateEntityException()
        {
            // Arrange
            var travelId = 1;
            var tripRequestRequest = new TripRequestRequest
            {
                PassengerId = "2",
                TravelId = travelId
            };

            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var passenger = TestHelpers.TestHelper.GetTestUserFourBlocked();
            var travel = new Travel
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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetByIdAsync(passenger.Id))
                .ReturnsAsync(passenger);

            var travelRepositoryMock = new Mock<ITravelRepository>();
            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(travelId))
                .ReturnsAsync(travel);
            travelRepositoryMock.Setup(repo => repo.CreateTravelAsync(It.IsAny<Travel>()))
                .ThrowsAsync(new UnauthorizedOperationException("You cannot create travel"));

            var tripRequestValidatorMock = new Mock<ITripRequestValidator>();
            tripRequestValidatorMock.Setup(validator => validator.ValidateIfPassengerAlreadyCreateTripRequest(tripRequestRequest))
                .ReturnsAsync(true);
            var userValidationMock = new Mock<IUserValidation>();
            userValidationMock.Setup(validator => validator.ValidateUserNotBanned(loggedUser))
                .ReturnsAsync(false);

            var tripRequestService = new TripRequestService(
                null,
                null,
                travelRepositoryMock.Object,
                userRepositoryMock.Object,
                null);

            // Act & Assert

            Assert.ThrowsExceptionAsync<DuplicateEntityException>(async () =>
            {
                await tripRequestService.CreateAsync(passenger, It.IsAny<TripRequestRequest>());
            });
        }
    }
}
