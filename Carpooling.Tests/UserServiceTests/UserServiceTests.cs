using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.AdminModels;
using Carpooling.BusinessLayer.Helpers;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.UserServiceTests
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService sut;

        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IMapper> userMapperMock;
        private Mock<IUserValidation> userValidatorMock;
        private Mock<CarPoolingDbContext> dbContextMock;
        private Mock<IIdentityHelper> identityHelperMock;
        private Mock<UserManager<User>> userManagerMock;
        private Mock<IUserStore<User>> userStoreMock;

        [TestInitialize]

        public void Initialize()
        {
            userStoreMock = new Mock<IUserStore<User>>();
            userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            identityHelperMock = new Mock<IIdentityHelper>();
            dbContextMock = new Mock<CarPoolingDbContext>();
            userValidatorMock = new Mock<IUserValidation>();
            userMapperMock = new Mock<IMapper>();
            userRepositoryMock = new Mock<IUserRepository>();

            SetupUserRepositoryMock();

            SetupUserValidatorMock();

            userManagerMock
                .Setup(x => x.DeleteAsync(It.IsAny<User>()));


            sut = new UserService(userRepositoryMock.Object,
                 userMapperMock.Object,
                 userManagerMock.Object,
                 dbContextMock.Object,
                 userValidatorMock.Object,
                 identityHelperMock.Object);
        }

        private void SetupUserRepositoryMock()
        {
            userRepositoryMock
                .Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));

            userRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new User()));
           
            userRepositoryMock
                .Setup(x => x.UnBanUser(It.IsAny<User>()))
                .Returns(Task.FromResult("User successfully UnBanned"));

            userRepositoryMock
                .Setup(x => x.BanUser(It.IsAny<User>()))
                .Returns(Task.FromResult("User successfully banned"));
        }
        private void SetupUserValidatorMock()
        {
            userValidatorMock
                .Setup(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()));

            userValidatorMock
                .Setup(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()));
            
            userValidatorMock
                .Setup(x => x.ValidateIfUsernameExist(It.IsAny<string>()));

            userValidatorMock
                .Setup(x => x.ValidateUserNotBanned(It.IsAny<User>()));

            userValidatorMock
                .Setup(x => x.ValidateUserAlreadyBanned(It.IsAny<User>()));

        }
        [TestMethod]

        public async Task GetByUsernameAuthAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.GetByUsernameAuthAsync("Gosho");

            //Verify
            userRepositoryMock.Verify(x => x.GetByUsernameAsync("Gosho"), Times.Once);
        }

        [TestMethod]

        public async Task DeleteAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.DeleteAsync(It.IsAny<User>(), It.IsAny<string>());

            //Verify
            userValidatorMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>()), Times.Once);

            userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public async Task DeleteAsync_ShouldReturn()
        {
            //Act
            var result = await sut.DeleteAsync(It.IsAny<User>(), It.IsAny<string>());

            //Verify
            userValidatorMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>()), Times.Once);

            userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);

            StringAssert.Contains(result, "User successfully deleted.");
        }

        [TestMethod]

        public async Task UnBanUser_ShouldInvoke()
        {
            //Act
            var result = await sut.UnBanUser(new User(), new BanOrUnBanDto());

            //Verify
            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsernameAsync(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserNotBanned(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.UnBanUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public async Task UnBanUser_ShouldReturn()
        {
            //Act
            var result = await sut.UnBanUser(new User(), new BanOrUnBanDto());

            //Verify
            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsernameAsync(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserNotBanned(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.UnBanUser(It.IsAny<User>()), Times.Once);

            StringAssert.Contains(result, "User successfully UnBanned");
        }

        [TestMethod]

        public async Task BanUser_ShouldInvoke()
        {
            //Act
            var result = await sut.BanUser(new User(), new BanOrUnBanDto());

            //Verify
            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsernameAsync(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserAlreadyBanned(It.IsAny<User>()));

            userRepositoryMock.Verify(x => x.BanUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public async Task BanUser_ShouldReturn()
        {
            //Act
            var result = await sut.BanUser(new User(), new BanOrUnBanDto());

            //Verify
            userValidatorMock.Verify(x => x.ValidateIfUsernameExist(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateLoggedUserIsAdmin(It.IsAny<User>()), Times.Once);

            userRepositoryMock.Verify(x => x.GetByUsernameAsync(It.IsAny<string>()), Times.Once);

            userValidatorMock.Verify(x => x.ValidateUserAlreadyBanned(It.IsAny<User>()));

            userRepositoryMock.Verify(x => x.BanUser(It.IsAny<User>()), Times.Once);

            StringAssert.Contains(result, "User successfully banned");
        }


    }
}
