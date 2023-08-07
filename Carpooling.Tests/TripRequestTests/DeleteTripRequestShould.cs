using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using CarPooling.Data.Repositories;
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
    public class DeleteTripRequestShould
    {
        private Mock<ITripRequestRepository> tripRequestRepositoryMock;
        private Mock<IUserValidation> userValidationMock;
        private TripRequestService tripRequestService;
        private Mock<ITravelRepository> travelRepositoryMock;

        [TestInitialize]
        public void Initialize()
        {
            tripRequestRepositoryMock = new Mock<ITripRequestRepository>();
            userValidationMock = new Mock<IUserValidation>();
            travelRepositoryMock = new Mock<ITravelRepository>();
            tripRequestService = new TripRequestService(
                tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                travelRepositoryMock.Object, // Mock other required dependencies as needed or pass null
                null,
                null);
        }

        [TestMethod]
        public async Task DeleteAsync_ValidRequest_ReturnsSuccessMessage()
        {
            // Arrange
            var tripRequestId = 1; // Replace with actual trip request ID
            var tripRequest = new TripRequest
            {
                Id = tripRequestId,
                PassengerId = "2",
                Status = TripRequestEnum.Approved,
                TravelId=1
               
            };
            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();

            tripRequestRepositoryMock.Setup(repo => repo.GetByIdAsync(tripRequestId)).ReturnsAsync(tripRequest);
            userValidationMock.Setup(validation => validation.ValidateUserLoggedAndAdmin(loggedUser, tripRequest.PassengerId)).Verifiable();
            tripRequestRepositoryMock.Setup(repo => repo.DeleteAsync(tripRequestId)).ReturnsAsync("Trip request successfully deleted.");
            travelRepositoryMock.Setup(repo => repo.RemoveUserToTravelAsync(tripRequest.TravelId, tripRequest.PassengerId));
            // Act
            var result = await tripRequestService.DeleteAsync(loggedUser, tripRequestId);

            // Assert
            Assert.AreEqual("Trip request successfully deleted.", result);
            tripRequestRepositoryMock.Verify(repo => repo.GetByIdAsync(tripRequestId), Times.Once);
            userValidationMock.Verify(validation => validation.ValidateUserLoggedAndAdmin(loggedUser, tripRequest.PassengerId), Times.Once);
            tripRequestRepositoryMock.Verify(repo => repo.DeleteAsync(tripRequestId), Times.Once);
        }
        [TestMethod]
        public async Task DeleteAsync_UserIsNotAuthorized_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var tripRequestId = 1;
            var tripRequest = new TripRequest
            {
                Id = tripRequestId,
                PassengerId = "2",
                Status = TripRequestEnum.Approved
            };
            var loggedUser = TestHelpers.TestHelper.GetTestUserFourBlocked();

            tripRequestRepositoryMock.Setup(repo => repo.GetByIdAsync(tripRequestId)).ReturnsAsync(tripRequest);
            userValidationMock.Setup(validation => validation.ValidateUserLoggedAndAdmin(loggedUser, tripRequest.PassengerId))
                .ThrowsAsync(new UnauthorizedOperationException("You are banned"));

            // Act & Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(() 
                => tripRequestService.DeleteAsync(loggedUser, tripRequestId));
            tripRequestRepositoryMock.Verify(repo => repo.GetByIdAsync(tripRequestId), Times.Once);
            userValidationMock.Verify(validation => validation.ValidateUserLoggedAndAdmin(loggedUser, tripRequest.PassengerId), Times.Once);
            tripRequestRepositoryMock.Verify(repo => repo.DeleteAsync(tripRequestId), Times.Never);
        }
    }
}
