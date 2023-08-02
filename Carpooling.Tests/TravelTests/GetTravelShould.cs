using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Models;
using Carpooling.Service.Dto_s.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Repositories.Contracts;
using Moq;

namespace Carpooling.Tests.TravelTests
{
    [TestClass]
    public class GetTravelShould
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
        public async Task GetByIdAsync_ReturnsTravelResponse()
        {
            //Arrange
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
            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(travel);
            mapperMock.Setup(mapper => mapper.Map<TravelResponse>(It.IsAny<Travel>()))
                .Returns((Travel travel) => MapTravelToTravelResponse(travel));
            
            var travelService = new TravelService(
                travelRepositoryMock.Object,
                mapperMock.Object,
                addressRepositoryMock.Object,
                carRepositoryMock.Object,
                travelValidatorMock.Object,
                userValidationMock.Object);

            // Act
            var result = await travelService.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            travelRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            Assert.IsInstanceOfType(result, typeof(TravelResponse));
            Assert.AreEqual(travel.StartLocation.Details, result.StartLocationName);
            Assert.AreEqual(travel.EndLocation.Details, result.DestinationName);
            Assert.AreEqual(travel.DepartureTime, result.DepartureTime);
        }
        private TravelResponse MapTravelToTravelResponse(Travel travel)
        {
            var travelResponse = new TravelResponse
            {
                StartLocationName = travel.StartLocation.Details,
                DestinationName = travel.EndLocation.Details,
                DepartureTime = (DateTime)travel.DepartureTime,
                ArrivalTime = (DateTime)travel.ArrivalTime,
                AvailableSeats = (int)travel.AvailableSeats,
                IsComplete = (bool)travel.IsCompleted,
                CarRegistration = travel.Car.Registration
            };

            return travelResponse;
        }
    }
}
