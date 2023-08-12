using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Carpooling.Tests.FeedbackServiceTests
{
    [TestClass]
    public class FeedbackServiceTests
    {
        private FeedbackService sut;

        private Mock<IFeedbackRepository> feedbackRepositoryMock;
        private Mock<UserManager<User>> userManagerMock;
        private Mock<IUserStore<User>> userStoreMock;
        private Mock<ITravelRepository> travelRepositoryMock;

        [TestInitialize]

        public void Initialize()
        {
            feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            userStoreMock = new Mock<IUserStore<User>>();
            userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            travelRepositoryMock = new Mock<ITravelRepository>();

            feedbackRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Feedback>()))
                .Returns(Task.FromResult(new Feedback()));

            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback()));

            feedbackRepositoryMock
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(new List<Feedback>()));

            sut = new FeedbackService(feedbackRepositoryMock.Object, 
                userManagerMock.Object, 
                travelRepositoryMock.Object);
        }

        [TestMethod]

        public async Task CreateAsync_ShouldInvoke()
        {

            //Arrange
            var feedback = new Feedback { Passenger = new User { Id = "123" } };

            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsCompleted = true }));

            //Act
            var result = await sut.CreateAsync(feedback, new User{ IsBlocked = false});

            //verify
            travelRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);

            feedbackRepositoryMock.Verify(x => x.CreateAsync(feedback), Times.Once);
        }

        [TestMethod]

        public async Task CreateAsync_WhenTravelNotCompleted_ShouldThrow()
        {
            //Arrange
            var feedback = new Feedback { Passenger = new User { Id = "123" } };

            travelRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Travel() { IsCompleted = false }));

            //Act
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(
                () => sut.CreateAsync(feedback, new User { IsBlocked = false }));

            //Verify
            Assert.AreEqual("Feedback can only be left on completed trips!", exception.Message);

        }

        [TestMethod]

        public async Task CreateAsync_WhenUserBlocked_ShouldThrow()
        {

            //Arrange
            var feedback = new Feedback { Passenger = new User { Id = "123" } };

            //Act
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(
                () => sut.CreateAsync(feedback, new User { IsBlocked = true }));

            //verify
            Assert.AreEqual("Only non-blocked user can make feedbacks!", exception.Message);
        }

        [TestMethod]

        public async Task GetByIdAsync_ShouldInvoke()
        {

            //Act
            var result = await sut.GetByIdAsync(1);

            //Verify
            feedbackRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once());
        }

        [TestMethod]

        public async Task GetAllAsync_ShouldInvoke()
        {

            //Act
            var result = await sut.GetAllAsync();

            //Verify
            feedbackRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once());
        }

        [TestMethod]

        public async Task DeleteAsync_ShouldInvoke()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "123" }, PassengerId = "123" }));

            feedbackRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());
            
            var user = new User { IsBlocked = false, Id = "123" };

            //Act
            var result = await sut.DeleteAsync(1, user);

            //verify
            feedbackRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);

            userManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Once);

            feedbackRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]

        public async Task DeleteAsync_WhenUserBlocked_ShouldThrow()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "123" }, PassengerId = "123" }));

            feedbackRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            var user = new User { IsBlocked = true, Id = "123" };

            //Act
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(
                () => sut.DeleteAsync(1, user));

            //verify
            Assert.AreEqual("You do not have permission to delete this feedback!", exception.Message);
        }

        [TestMethod]

        public async Task DeleteAsync_WhenUserInvalidIdMatch_ShouldThrow()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "12" }, PassengerId = "12" }));

            feedbackRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            var user = new User { IsBlocked = false, Id = "123" };

            //Act
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(
                () => sut.DeleteAsync(1, user));

            //verify
            Assert.AreEqual("You do not have permission to delete this feedback!", exception.Message);
        }

        [TestMethod]

        public async Task DeleteAsync_WhenUserInvalidIdMatchButAdmin_ShouldInvoke()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "12" }, PassengerId = "12" }));

            feedbackRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() { "Administrator" });

            var user = new User { IsBlocked = false, Id = "123" };

            //Act
            var result = await sut.DeleteAsync(1, user);

            //verify
            feedbackRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);

            userManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Once);

            feedbackRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]

        public async Task UpdateAsync_ShouldInvoke()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "123" }, PassengerId = "123" }));

            feedbackRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Feedback>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            var user = new User { IsBlocked = false, Id = "123" };
            var feedback = new Feedback();

            //Act
            var result = await sut.UpdateAsync(1, user, feedback);

            //verify
            feedbackRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);

            userManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Once);

            feedbackRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Feedback>()), Times.Once);
        }

        [TestMethod]

        public async Task UpdateAsync_WhenUserBlocked_ShouldThrow()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "123" }, PassengerId = "123" }));

            feedbackRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Feedback>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            var user = new User { IsBlocked = true, Id = "123" };
            var feedback = new Feedback();

            //Act
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(
                () => sut.UpdateAsync(1, user, feedback));

            //verify
            Assert.AreEqual("You do not have permission to delete this feedback!", exception.Message);
        }

        [TestMethod]

        public async Task UpdateAsync_WhenUserInvalidIdMatch_ShouldThrow()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "12" }, PassengerId = "12" }));

            feedbackRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Feedback>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>());

            var user = new User { IsBlocked = false, Id = "123" };
            var feedback = new Feedback();

            //Act
            var exception = await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(
                () => sut.UpdateAsync(1, user, feedback));

            //verify
            Assert.AreEqual("You do not have permission to delete this feedback!", exception.Message);
        }

        [TestMethod]

        public async Task UpdateAsync_WhenUserInvalidIdMatchButAdmin_ShouldInvoke()
        {

            //Arrange
            feedbackRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Feedback { Passenger = new User { Id = "12" }, PassengerId = "12" }));

            feedbackRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Feedback>()))
                .Returns(Task.FromResult(new Feedback()));

            userManagerMock
                .Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(new List<string>() { "Administrator" });

            var user = new User { IsBlocked = false, Id = "123" };
            var feedback = new Feedback();

            //Act
            var result = await sut.UpdateAsync(1, user, feedback);

            //verify
            feedbackRepositoryMock.Verify(x => x.GetByIdAsync(1), Times.Once);

            userManagerMock.Verify(x => x.GetRolesAsync(It.IsAny<User>()), Times.Once);

            feedbackRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<Feedback>()), Times.Once);
        }
    }
}
