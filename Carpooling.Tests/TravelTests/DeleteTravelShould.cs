using AutoMapper;
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
    public class DeleteTravelShould
    {
        private Mock<ITravelRepository> travelRepositoryMock;
        private Mock<IMapper> mapperMock;
        private Mock<IAddressRepository> addressRepositoryMock;
        private Mock<ICarRepository> carRepositoryMock;
        private Mock<ITravelValidator> travelValidatorMock;
        private Mock<IUserValidation> userValidationMock;
        private Mock<IMapService> mapServiceMock;

        [TestInitialize]
        public void Setup()
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
        public async Task DeleteAsync_ValidTravelId_DeletesTravel()
        {
            // Arrange

            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
            var travelToDelete = new Travel 
            {
                Id=1,
                Driver = TestHelpers.TestHelper.GetTestUserOne(),
                StartLocation = TestHelpers.TestHelper.GetTestAddressOne(),
                EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                Car = TestHelpers.TestHelper.GetTestCarTwo(),
                ArrivalTime = DateTime.Now,
                DepartureTime = DateTime.Now,
                IsCompleted = false
            };

            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(travelToDelete.Id))
                .ReturnsAsync(travelToDelete);

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object,
                addressRepositoryMock.Object, carRepositoryMock.Object,
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

            // Act
            var result = await travelService.DeleteAsync(loggedUser, travelToDelete.Id);

            // Assert
            travelRepositoryMock.Verify(repo => repo.GetByIdAsync(travelToDelete.Id), Times.Once);
            travelRepositoryMock.Verify(repo=>repo.DeleteAsync(travelToDelete.Id));
        }

        [TestMethod]
        public async Task DeleteAsync_InvalidTravelId_ThrowsEntityNotFoundException()
        {
            // Arrange
            var loggedUser = TestHelpers.TestHelper.GetTestUserOne();

            travelRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new EntityNotFoundException("Not found"));

            var travelService = new TravelService(travelRepositoryMock.Object, mapperMock.Object, 
                addressRepositoryMock.Object, carRepositoryMock.Object, 
                travelValidatorMock.Object, userValidationMock.Object, mapServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await travelService.DeleteAsync(loggedUser, It.IsAny<int>());
            });
        }
    }
}
