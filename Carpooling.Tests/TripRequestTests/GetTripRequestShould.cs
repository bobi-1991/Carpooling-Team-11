using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Responses;
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
    public class GetTripRequestShould
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
        public async Task GetByIdAsync_ValidId_ReturnsTripRequestResponse()
        {
            // Arrange
            var tripRequest = new TripRequest
            {
                Id = 1,
                Passenger = TestHelpers.TestHelper.GetTestUserOne(),
                Driver = TestHelpers.TestHelper.GetTestUserTwo(),
                Travel = new Travel
                {
                    StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                    EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                    DepartureTime = DateTime.Now,
                },
                Status = TripRequestEnum.Approved
            };

            tripRequestRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(tripRequest);

            var tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                null, null, null);
            // Act
            var result = await tripRequestService.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("testOne@gmail.com", result.PassengerUsername);
            Assert.AreEqual("Ovcha Kupel", result.StartLocationDetails);
            Assert.AreEqual("Lyulin 5", result.EndLocationDetails);
            Assert.AreEqual(tripRequest.Travel.DepartureTime, result.DepartureTime);
            Assert.AreEqual(TripRequestEnum.Approved.ToString(), result.Status.ToString());
        }
        [TestMethod]
        public async Task GetByIdAsync_InvalidId_ThrowsException()
        {
            // Arrange
            tripRequestRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ThrowsAsync(new EntityNotFoundException("Not Found!"));
            var tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                null, null, null);
            // Act and Assert
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await tripRequestService.GetByIdAsync(It.IsAny<int>()));
        }
    }
}
