using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Exceptions;
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
    public class GetAllTripRequestsShould
    {
        private Mock<ITripRequestRepository> tripRequestRepositoryMock;
        private Mock<IUserValidation> userValidationMock;
        private TripRequestService tripRequestService;

        [TestInitialize]
        public void Initialize()
        {
            tripRequestRepositoryMock = new Mock<ITripRequestRepository>();
            userValidationMock = new Mock<IUserValidation>();
            tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                null,
                null,
                null);
        }
        [TestMethod]
        public async Task GetAllAsync_ValidRequest_ReturnsTripRequestResponses()
        {
            // Arrange
            var tripRequest1 = new TripRequest
            {
                Id = 1,
                Passenger = TestHelpers.TestHelper.GetTestUserTwo(),
                Driver = TestHelpers.TestHelper.GetTestUserOne(),
                Travel = new Travel
                {
                    StartLocation = TestHelpers.TestHelper.GetTestAddressThree(),
                    EndLocation = TestHelpers.TestHelper.GetTestAddressFour(),
                    DepartureTime = DateTime.Now,
                },
                Status = TripRequestEnum.Approved
            };

            var tripRequest2 = new TripRequest
            {
                Id = 2,
                Passenger = TestHelpers.TestHelper.GetTestUserOne(),
                 Driver = TestHelpers.TestHelper.GetTestUserOne(),
                Travel = new Travel
                {
                    StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                    EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                    DepartureTime = DateTime.Now.AddDays(1),
                },
                Status = TripRequestEnum.Approved
            };

            var tripRequests = new List<TripRequest> { tripRequest1, tripRequest2 };

            tripRequestRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(tripRequests);

            var tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                null, null, null);
            // Act
            var result = await tripRequestService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            var tripRequestResponse1 = result.ElementAt(0);
            Assert.AreEqual("testTwo@gmail.com", tripRequestResponse1.PassengerUsername);
            Assert.AreEqual("Malinova Dolina", tripRequestResponse1.StartLocationDetails);
            Assert.AreEqual("Centar", tripRequestResponse1.EndLocationDetails);
            Assert.AreEqual(tripRequest1.Travel.DepartureTime, tripRequestResponse1.DepartureTime);
            Assert.AreEqual(TripRequestEnum.Approved.ToString(), tripRequestResponse1.Status.ToString());

            var tripRequestResponse2 = result.ElementAt(1);
            Assert.AreEqual("testOne@gmail.com", tripRequestResponse2.PassengerUsername);
            Assert.AreEqual("Ovcha Kupel", tripRequestResponse2.StartLocationDetails);
            Assert.AreEqual("Lyulin 5", tripRequestResponse2.EndLocationDetails);
            Assert.AreEqual(tripRequest2.Travel.DepartureTime, tripRequestResponse2.DepartureTime);
            Assert.AreEqual(TripRequestEnum.Approved.ToString(), tripRequestResponse2.Status.ToString());
        }
        [TestMethod]
        public async Task GetAllAsync_NoTripRequests_ReturnsEmptyList()
        {
            // Arrange
            var tripRequests = new List<TripRequest>(); // Empty list

            tripRequestRepositoryMock.Setup(repo => repo.GetAllAsync()).ThrowsAsync(new EmptyListException("No requests yet!"));
            var tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                null, null, null);
            // Act && Assert
            await Assert.ThrowsExceptionAsync<EmptyListException>(() =>
                tripRequestService.GetAllAsync());
        }
    }
}
