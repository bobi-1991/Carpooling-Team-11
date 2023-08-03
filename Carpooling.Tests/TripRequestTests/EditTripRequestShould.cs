using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using CarPooling.Data.Repositories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.TripRequestTests
{
    [TestClass]
    public class EditTripRequestShould
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

        //[TestMethod]
        //public async Task EditRequestAsync_ValidData_ReturnsResult()
        //{
        //    // Arrange
        //    var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
        //    var tripId = 1;
        //    var answer = "approve";
        //    var tripRequestToUpdate = new TripRequest
        //    {
        //        Id = tripId,
        //        Passenger = TestHelpers.TestHelper.GetTestUserTwo(),
        //        PassengerId="2",
        //        Driver = TestHelpers.TestHelper.GetTestUserOne(),
        //        DriverId="1",
        //        Travel = new Travel
        //        {
        //            Id = 1,
        //            DriverId="1",
        //            StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
        //            EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
        //            DepartureTime = DateTime.Now,
        //            AvailableSeats = 2
        //        },
        //        TravelId=1,
        //        Status = TripRequestEnum.Pending
        //    };
        //    var travel = tripRequestToUpdate.Travel;

        //    userValidationMock.Setup(validator => validator.ValidateUserLoggedAndAdmin(loggedUser, tripRequestToUpdate.Travel.DriverId))
        //        .Verifiable();
        //    tripRequestRepositoryMock.Setup(repo => repo.GetByIdAsync(tripId))
        //        .ReturnsAsync(tripRequestToUpdate);
        //    travelRepositoryMock.Setup(repo => repo.GetByIdAsync(tripRequestToUpdate.TravelId))
        //        .ReturnsAsync(travel);
        //    tripRequestValidatorMock.Setup(validator => validator.ValidateStatusOfTripRequest(tripRequestToUpdate, answer))
        //        .ReturnsAsync("approve");

        //    // Act
        //    var result = await tripRequestService.EditRequestAsync(loggedUser, tripId, answer);

        //    // Assert
        //    Assert.AreEqual("approve", result);
        //    travelRepositoryMock.Verify(repo => repo.AddUserToTravelAsync(travel.Id, tripRequestToUpdate.PassengerId), Times.Once);
        //    tripRequestRepositoryMock.Verify(repo => repo.EditRequestAsync(tripRequestToUpdate, "approve"), Times.Once);
        //}
    }
}
