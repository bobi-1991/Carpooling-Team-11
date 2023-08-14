using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.AdminModels;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.BusinessLayer.Helpers;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Data;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Moq;

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

            SetupUserManagerMock();

            sut = new UserService(userRepositoryMock.Object,
                 userMapperMock.Object,
                 userManagerMock.Object,
                 dbContextMock.Object,
                 userValidatorMock.Object,
                 identityHelperMock.Object);
        }

        private void SetupUserManagerMock()
        {
            userManagerMock
                .Setup(x => x.DeleteAsync(It.IsAny<User>()));

            userManagerMock
                .Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));

            userManagerMock
                .Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()));
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

            userRepositoryMock
                .Setup(x => x.TravelHistoryAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<Travel>());

            userRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<User>());
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

        [TestMethod]

        public async Task TravelHistoryAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.TravelHistoryAsync(new User(), "123");

            //Verify
            userValidatorMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), "123"), Times.Once);

            userRepositoryMock.Verify(x => x.TravelHistoryAsync("123"), Times.Once);
        }

        [TestMethod]

        public async Task GetByIdAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.GetByIdAsync("123");

            //Verify
            userRepositoryMock.Verify(x => x.GetByIdAsync("123"), Times.Once);
        }

        [TestMethod]

        public async Task GetByUsernameAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.GetByUsernameAsync("Trendafil");

            //Verify
            userRepositoryMock.Verify(x => x.GetByUsernameAsync("Trendafil"), Times.Once);
        }

        [TestMethod]

        public async Task GetAllAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.GetAllAsync();

            //Verify
            userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [TestMethod]

        public async Task RegisterAsync_ShouldInvoke()
        {
            //Arrange
            var user = new User() { Email = "abv@abv.bg", UserName = "TestTest" };
            var password = "Password2@";

            userMapperMock
                .Setup(x => x.Map<User>(It.IsAny<UserRequest>()))
                .Returns(user);

            userManagerMock
                .Setup(x => x.CreateAsync(user, password))
                .ReturnsAsync(IdentityResult.Success);

            var result2 = new IdentityResult();
            //Act
            var result = await sut.RegisterAsync(new UserRequest() { Password = "Password2@", Email = "abv@abv.bg" });

            //Verify
            userManagerMock.Verify(x => x.CreateAsync(user, password), Times.Once);

            userManagerMock.Verify(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task RegisterAsync_ShouldThrow()
        {
            //Arrange
            var user = new User() { Email = "abv@abv.bg", UserName = "TestTest" };
            var password = "Password2@";

            userMapperMock
                .Setup(x => x.Map<User>(It.IsAny<UserRequest>()))
                .Returns(user);

            userManagerMock
                .Setup(x => x.CreateAsync(user, password))
                .ReturnsAsync(new IdentityResult());

            //Verify
            var exception = await Assert.ThrowsExceptionAsync<NullReferenceException>(
                () => sut.RegisterAsync(new UserRequest() { Password = "Password2@", Email = "abv@abv.bg" }));
        }

        [TestMethod]

        public async Task UpdateAsync_WhenRoleExists_ShouldInvokeChangeRole()
        {
            userRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()))
                .Returns(Task.FromResult(new User()));

            identityHelperMock
                .Setup(x => x.ChangeRole(It.IsAny<User>(), It.IsAny<User>(), It.IsAny<string>()));

            //Act
            var result = await sut.UpdateAsync(new User(), "123", new UserUpdateDto() { Role = "Driver" });

            //Verify
            userValidatorMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), "123"), Times.Once);

            userRepositoryMock.Verify(x => x.GetByIdAsync("123"), Times.Once);

            userRepositoryMock.Verify(x => x.UpdateAsync("123", It.IsAny<User>()), Times.Once);

            identityHelperMock.Verify(x => x.ChangeRole(It.IsAny<User>(), It.IsAny<User>(), It.IsAny<string>()), Times.Once);

        }

        [TestMethod]

        public async Task UpdateAsync_ShouldInvoke()
        {
            userRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<User>()))
                .Returns(Task.FromResult(new User()));

            //Act
            var result = await sut.UpdateAsync(new User(), "123", new UserUpdateDto());

            //Verify
            userValidatorMock.Verify(x => x.ValidateUserLoggedAndAdmin(It.IsAny<User>(), "123"), Times.Once);

            userRepositoryMock.Verify(x => x.GetByIdAsync("123"), Times.Once);

            userRepositoryMock.Verify(x => x.UpdateAsync("123", It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public async Task DeleteUserWhenAdminAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.DeleteUserWhenAdminAsync(It.IsAny<string>());

            //Verify
            userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);

            userManagerMock.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public async Task GetAllUsersAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.GetAllUsersAsync();

            //Verify
            userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [TestMethod]

        public async Task GetUserByIdAsync_ShouldInvoke()
        {
            //Act
            var result = await sut.GetUserByIdAsync(It.IsAny<string>());

            //Verify
            userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]

        public async Task BanUserById_ShouldInvoke()
        {
            //Act
            var result = await sut.BanUserById(It.IsAny<string>());

            //Verify
            userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.BanUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public async Task UnbanUserById_ShouldInvoke()
        {
            //Act
            var result = await sut.UnbanUserById(It.IsAny<string>());

            //Verify
            userRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<string>()), Times.Once);

            userRepositoryMock.Verify(x => x.UnBanUser(It.IsAny<User>()), Times.Once);
        }

        [TestMethod]

        public async Task CovertToAdministrator_ShouldInvoke()
        {
            //Arrage
            userRepositoryMock
                .Setup(x => x.ConvertToAdministrator(It.IsAny<string>()));

            //Act
            await sut.ConvertToAdministrator(It.IsAny<string>());

            //Verify
            userRepositoryMock.Verify(x => x.ConvertToAdministrator(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]

        public async Task TopPassengers_ShouldInvoke()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetTopPassengers(It.IsAny<List<User>>(), 2))
                .ReturnsAsync(new List<User>());

            identityHelperMock
                .Setup(x => x.GetRole(It.IsAny<User>()));

            //Act
            var result = await sut.TopPassengers(2);

            //Verify
            userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);

            //identityHelperMock.Verify(x => x.GetRole(It.IsAny<User>()), Times.Exactly(2));

            userRepositoryMock.Verify(x => x.GetTopPassengers(It.IsAny<List<User>>(), It.IsAny<int>()), Times.Once);
        }
        

        [TestMethod]

        public async Task TopTravelOrganizers_ShouldInvoke()
        {
            //Arrange
            userRepositoryMock
                .Setup(x => x.GetTopTravelOrganizers(It.IsAny<List<User>>(), 2))
                .ReturnsAsync(new List<User>());

            identityHelperMock
                .Setup(x => x.GetRole(It.IsAny<User>()));

            //Act
            var result = await sut.TopTravelOrganizers(2);

            //Verify
            userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);

            //identityHelperMock.Verify(x => x.GetRole(It.IsAny<User>()), Times.Exactly(2));

            userRepositoryMock.Verify(x => x.GetTopTravelOrganizers(It.IsAny<List<User>>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]

        public async Task TopTravelOrganizers_ShouldInvoke2()
        {
            //Arrange
            userManagerMock
                .Setup(x => x.AddToRolesAsync(It.IsAny<User>(), It.IsAny<List<string>>()));

            //var user1 = new User()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserName = "Gosho892",
            //    FirstName = "Gosho2",
            //    LastName = "Goshev2",
            //    Email = "gosho2@gmail.com",
            //    AverageRating = 4.5m,
            //    IsBlocked = false
            //};

            //var user2 = new User()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserName = "Gosho89",
            //    FirstName = "Gosho",
            //    LastName = "Goshev",
            //    Email = "gosho@gmail.com",
            //    AverageRating = 4.5m,
            //    IsBlocked = false
            //};

            userRepositoryMock
                .Setup(x => x.GetTopTravelOrganizers(It.IsAny<List<User>>(), 2))
                .ReturnsAsync(new List<User>());
                

            identityHelperMock
                .Setup(x => x.GetRole(It.IsAny<User>()))
                .ReturnsAsync(It.IsAny<string>());

            //Act
            var result = await sut.TopTravelOrganizers(2);

            //Verify
            userRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);

            //identityHelperMock.Verify(x => x.GetRole(It.IsAny<User>()), Times.Exactly(2));

            userRepositoryMock.Verify(x => x.GetTopTravelOrganizers(It.IsAny<List<User>>(), It.IsAny<int>()), Times.Once);
        }
    }
}
