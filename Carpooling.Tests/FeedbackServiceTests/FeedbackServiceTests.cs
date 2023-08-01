using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.FeedbackServiceTests
{
    [TestClass]
    public class FeedbackServiceTests
    {
        private FeedbackService sut;

        private Mock<IFeedbackRepository> feedbackRepositoryMock;
        private Mock<UserManager<User>> userManagerMock;
        private Mock<IUserStore<User>> userStoreMock;

        [TestInitialize]

        public void Initialize()
        {
            feedbackRepositoryMock = new Mock<IFeedbackRepository>();
            userStoreMock = new Mock<IUserStore<User>>();
            userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            feedbackRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Feedback>()))
                .Returns(Task.FromResult(new Feedback()));

            sut = new FeedbackService(feedbackRepositoryMock.Object, userManagerMock.Object);
        }

        [TestMethod]

        public async Task CreateAsync_ShouldInvoke()
        {
            //Arrange
            var feedback = new Feedback { Passenger = new User { Id = "123" } };

            //Act
            var result = await sut.CreateAsync(feedback, new User{ IsBlocked = false});

            //verify
            feedbackRepositoryMock.Verify(x => x.CreateAsync(feedback), Times.Once);
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

        //public async Task<Feedback> CreateAsync(Feedback feedback, User user)
        //{

        //    if (user.IsBlocked)
        //    {
        //        throw new UnauthorizedOperationException("Only non-blocked user can make feedbacks!");
        //    }

        //    feedback.Passenger = user;
        //    feedback.PassengerId = user.Id;

        //   return await _feedbackRepository.CreateAsync(feedback);
        //}


    }
}
