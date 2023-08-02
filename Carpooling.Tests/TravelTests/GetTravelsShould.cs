using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Carpooling.Service.Dto_s.Responses;
using Moq;
using AutoMapper;
using Microsoft.CodeAnalysis;
using Carpooling.BusinessLayer.Validation.Contracts;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;

namespace Carpooling.Tests.TravelTests
{
    [TestClass]
    public class GetTravelsShould
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
        public async Task GetAllAsync_ReturnsListOfTravelResponses()
        {
            // Arrange
            var travels = new List<Travel>
            {
                new Travel
                {
                    Driver = TestHelpers.TestHelper.GetTestUserOne(),
                    StartLocation = TestHelpers.TestHelper.GetTestAddressFour(),
                    EndLocation = TestHelpers.TestHelper.GetTestAddressThree(),
                    Car = TestHelpers.TestHelper.GetTestCarOne(),
                    DepartureTime=DateTime.Now,
                    ArrivalTime=DateTime.Now,
                    IsCompleted=true,
                },
                new Travel
                {
                    Driver = TestHelpers.TestHelper.GetTestUserOne(),
                    StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                    EndLocation =  TestHelpers.TestHelper.GetTestAddressTwo(),
                    Car = TestHelpers.TestHelper.GetTestCarTwo(),
                    ArrivalTime = DateTime.Now,
                    DepartureTime=DateTime.Now,
                    IsCompleted=false
                },
                
            };

            travelRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(travels);
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
            var result = await travelService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            travelRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<TravelResponse>));
            Assert.AreEqual(travels.Count, result.Count());
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
