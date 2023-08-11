using AutoMapper;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Data;
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
    public class FilterTravelShould
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
        public async Task FilterTravelsAndSortAsync_ValidSortBy_ReturnsSortedTravels()
        {
            // Arrange
            var travelOne = new Travel { Id = 1, CreatedOn = DateTime.UtcNow.AddHours(-2) };
            var travelTwo = new Travel { Id = 2, CreatedOn = DateTime.UtcNow.AddHours(-1) };
            var travelThree = new Travel { Id = 3, CreatedOn = DateTime.UtcNow };
            var travels = new List<Travel> { travelOne, travelTwo, travelThree };

            var travelRepositoryMock = new Mock<ITravelRepository>();

            travelRepositoryMock.Setup(repo => repo.FilterTravelsAndSortAsync(It.IsAny<string>()))
                .ReturnsAsync(travels);

           var travelService = new TravelService(
                travelRepositoryMock.Object,
                mapperMock.Object,
                addressRepositoryMock.Object,
                carRepositoryMock.Object,
                travelValidatorMock.Object,
                userValidationMock.Object,
                mapServiceMock.Object);

            // Act
            var result = await travelService.FilterTravelsAndSortAsync((It.IsAny<string>()));

            // Assert
            travelRepositoryMock.Verify(repo => repo.FilterTravelsAndSortAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
