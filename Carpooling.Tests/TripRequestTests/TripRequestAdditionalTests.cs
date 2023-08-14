using AutoMapper;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using CarPooling.Data.Models.ViewModels;
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
    public class TripRequestAdditionalTests
    {
        private TripRequestService sut;

        private Mock <ITravelRepository> travelRepositoryMock;
        private Mock <IMapper> mapperMock;
        private Mock <IUserValidation> userValidationMock;
        private Mock <ITripRequestRepository> tripRequestRepositoryMock;
        private Mock <IUserRepository> userRepositoryMock;
        private Mock <ITripRequestValidator> tripRequestValidatorMock;
        private Mock <IFeedbackRepository> feedbackRepositoryMock;

        [TestInitialize]

        public void Initialize()
        {
            travelRepositoryMock = new Mock<ITravelRepository>();
            mapperMock = new Mock<IMapper>();
            userValidationMock = new Mock<IUserValidation>();
            tripRequestRepositoryMock = new Mock<ITripRequestRepository>();
            userRepositoryMock = new Mock<IUserRepository>();
            tripRequestValidatorMock = new Mock<ITripRequestValidator>();
            feedbackRepositoryMock = new Mock<IFeedbackRepository>();

            tripRequestRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new TripRequest()));

            sut = new TripRequestService(tripRequestRepositoryMock.Object,
                userValidationMock.Object,
                travelRepositoryMock.Object,
                userRepositoryMock.Object,
                tripRequestValidatorMock.Object, 
                feedbackRepositoryMock.Object);
        }

        [TestMethod]

        public async Task GetByIdAsync_ShouldInvoke()
        {
            //Act
            var result = sut.GetByIdAsync(1);

            //Verify
            tripRequestRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]

        public async Task SeeAllHisPassengerRequestsMVCAsync_ShouldInvoke()
        {
            //Arrange
            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            tripRequestRepositoryMock
                .Setup(x => x.SeeAllHisPassengerRequestsMVCAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<TripRequest>());

            //Act
            var result = await sut.SeeAllHisPassengerRequestsMVCAsync(It.IsAny<User>(), It.IsAny<string>());

            //Verify
            userValidationMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            tripRequestRepositoryMock.Verify(x => x.SeeAllHisPassengerRequestsMVCAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]

        public async Task SeeAllHisPassengerRequestsAsync_ShouldInvoke()
        {
            //Arrange
            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            tripRequestRepositoryMock
                .Setup(x => x.SeeAllHisPassengerRequestsAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<TripRequest>());

            //Act
            var result = await sut.SeeAllHisPassengerRequestsAsync(It.IsAny<User>(), It.IsAny<string>());

            //Verify
            userValidationMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            tripRequestRepositoryMock.Verify(x => x.SeeAllHisPassengerRequestsAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]

        public async Task SeeAllHisDriverRequestsMVCAsync_ShouldInvoke()
        {
            //Arrange
            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            tripRequestRepositoryMock
                .Setup(x => x.SeeAllHisDriverRequestsMVCAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<TripRequest>());

            //Act
            var result = await sut.SeeAllHisDriverRequestsMVCAsync(It.IsAny<User>(), It.IsAny<string>());

            //Verify
            userValidationMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            tripRequestRepositoryMock.Verify(x => x.SeeAllHisDriverRequestsMVCAsync(It.IsAny<string>()), Times.Once);
        }

        //public async Task<IEnumerable<PassengersListViewModel>> SeeAllHisDriverPassengersMVCAsync(User loggedUser, string driverId)
        //{
        //    await this.userValidation.ValidateUserLoggedAndAdmin(loggedUser, driverId);

        //    var tripRequests = await this.tripRequestRepository.SeeAllHisDriverRequestsMVCAsync(driverId);

        //    var passengers = tripRequests
        //          .Select(tripRequest => tripRequest.Passenger)
        //          .Where(passenger => passenger != null && !passenger.IsDeleted)
        //          .ToList();

        //    var passengerViewModel = new List<PassengersListViewModel>();

        //    foreach (var trip in tripRequests.Where(x => x.Status == TripRequestEnum.Approved))
        //    {
        //        var passengerId = trip.PassengerId;
        //        var passenger = await this.userRepository.GetByIdAsync(passengerId);
        //        //NEW
        //        var feedbacks = await this.feedbackRepository.GetAllAsync();
        //        var currentFeedbacks = feedbacks.Where(x => x.PassengerId == trip.DriverId);

        //        passengerViewModel.Add(new PassengersListViewModel
        //        {
        //            Id = trip.PassengerId,
        //            DepartureTime = (DateTime)trip.Travel.DepartureTime,
        //            DepartureCity = trip.Travel.StartLocation.City,
        //            DepartureAddress = trip.Travel.StartLocation.Details,
        //            ArrivalCity = trip.Travel.EndLocation.City,
        //            ArrivalAddress = trip.Travel.EndLocation.Details,
        //            Username = trip.Passenger.UserName,
        //            AverageRating = trip.Passenger.AverageRating,
        //            Feedbacks = currentFeedbacks.Count()
        //        });
        //    }

        //    return passengerViewModel;
        //}

        [TestMethod]

        public async Task SeeAllHisDriverPassengersMVCAsync_ShouldInvoke()
        {
            //Arrange
            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            tripRequestRepositoryMock
                .Setup(x => x.SeeAllHisDriverRequestsMVCAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<TripRequest>());

            //Act
            var result = await sut.SeeAllHisDriverPassengersMVCAsync(It.IsAny<User>(), It.IsAny<string>());

            //Verify
            userValidationMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            tripRequestRepositoryMock.Verify(x => x.SeeAllHisDriverRequestsMVCAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
