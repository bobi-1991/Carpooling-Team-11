using AutoMapper;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Moq;

namespace Carpooling.Tests.TravelTests
{
    [TestClass]
    public class TravelServiceAdditionalTests
    {
        private TravelService sut;

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

            travelRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Travel>());

            sut = new TravelService(travelRepositoryMock.Object,
                 mapperMock.Object,
                 addressRepositoryMock.Object,
                 carRepositoryMock.Object,
                 travelValidatorMock.Object,
                 userValidationMock.Object,
                 mapServiceMock.Object);
        }

        [TestMethod]

        public async Task GetAllTravelAsync_ShouldInvoke()
        {
            //Act
            var result = sut.GetAllTravelAsync();

            //Verify
            travelRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [TestMethod]

        public async Task FilterTravelsAndSortForMVCAsync_ShouldInvoke()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.FilterTravelsAndSortAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Travel>());

            //Act
            var result = sut.FilterTravelsAndSortForMVCAsync(It.IsAny<string>());

            //Verify
            travelRepositoryMock.Verify(x => x.FilterTravelsAndSortAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]

        public async Task SetTravelToIsCompleteMVCAsync_ShouldInvoke()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsCompleted = false }));

            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            travelRepositoryMock
                .Setup(x => x.SetTravelToIsCompleteAsync(It.IsAny<Travel>()))
                .Returns(Task.FromResult("Travel successfully set to completed."));

            //Act
            var result = sut.SetTravelToIsCompleteMVCAsync(It.IsAny<User>(), It.IsAny<int>());

            //Verify
            travelRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);

            userValidationMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            travelRepositoryMock.Verify(x => x.SetTravelToIsCompleteAsync(It.IsAny<Travel>()), Times.Once);
        }

        [TestMethod]

        public async Task SetTravelToIsCompleteMVCAsync_WhenTravelCompleted_ShouldThrow()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsCompleted = true }));

            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            travelRepositoryMock
                .Setup(x => x.SetTravelToIsCompleteAsync(It.IsAny<Travel>()))
                .Returns(Task.FromResult("Travel successfully set to completed."));

            //Act && Verify
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(
                () => sut.SetTravelToIsCompleteMVCAsync(It.IsAny<User>(), It.IsAny<int>()));
        }

        [TestMethod]

        public async Task SetTravelToIsCompleteAsync_ShouldInvoke()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsCompleted = false }));

            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            travelRepositoryMock
                .Setup(x => x.SetTravelToIsCompleteAsync(It.IsAny<Travel>()))
                .Returns(Task.FromResult("Travel successfully set to completed."));

            //Act
            var result = sut.SetTravelToIsCompleteAsync(It.IsAny<User>(), It.IsAny<int>());

            //Verify
            travelRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);

            userValidationMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            travelRepositoryMock.Verify(x => x.SetTravelToIsCompleteAsync(It.IsAny<Travel>()), Times.Once);
        }

        [TestMethod]

        public async Task SetTravelToIsCompleteAsync_WhenTravelIsCompleted_ShouldReturn()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsCompleted = true }));

            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            travelRepositoryMock
                .Setup(x => x.SetTravelToIsCompleteAsync(It.IsAny<Travel>()))
                .Returns(Task.FromResult("Travel successfully set to completed."));

            //Act
            var result = sut.SetTravelToIsCompleteAsync(It.IsAny<User>(), It.IsAny<int>());

            //Verify
            Assert.AreEqual("This travel is already completed.", result.Result.ToString());
        }

        [TestMethod]

        public async Task DeleteMVCAsync_ShouldInvoke()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsDeleted = false }));

            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            travelRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult("Travel successfully deleted."));

            //Act
            var result = sut.DeleteMVCAsync(It.IsAny<User>(), It.IsAny<int>());

            //Verify
            travelRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);

            userValidationMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            travelRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]

        public async Task DeleteMVCAsync_IfTravelIsDeleted_ShouldReturn()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsDeleted = true }));

            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            travelRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult("Travel successfully deleted."));

            //Act && Verify
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(
                () => sut.DeleteMVCAsync(It.IsAny<User>(), It.IsAny<int>()));
        }

        [TestMethod]

        public async Task DeleteAsync_IfTravelIsDeleted_ShouldReturn()
        {
            //Arrange
            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsDeleted = true }));

            userValidationMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            travelRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult("Travel successfully deleted."));

            //Act
            var result = sut.DeleteAsync(It.IsAny<User>(), It.IsAny<int>());

            //Verify
            Assert.AreEqual("This travel is already deleted.", result.Result.ToString());
        }
    }
}
