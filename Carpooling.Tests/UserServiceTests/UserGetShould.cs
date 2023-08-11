using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Moq;

namespace Carpooling.Tests.UserServiceTests
{
    [TestClass]
    public class UserGetShould
    {
        private UserService _userService;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IUserValidation> _mockUserValidator;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserValidator = new Mock<IUserValidation>();
            _userService = new UserService(
                _mockUserRepository.Object,
                null,
                null, 
                null, 
                _mockUserValidator.Object,
                null
            );
        }

        [TestMethod]
        public async Task GetByEmailAsync_ValidEmail_ReturnsUserResponse()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = email,
                AverageRating = 4
            };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetByEmailAsync(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.FirstName, result.FirstName);
            Assert.AreEqual(expectedUser.LastName, result.LastName);
            Assert.AreEqual(expectedUser.UserName, result.Username);
            Assert.AreEqual(expectedUser.Email, result.Email);
            Assert.AreEqual(expectedUser.AverageRating, result.AverageRating);
        }
        [TestMethod]
        public async Task GetByEmailAsync_WhenUserNotFound_ThrowsAsync()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = email,
                AverageRating = 4
            };
            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(email))
                .ThrowsAsync(new EntityNotFoundException("User not found!"));
            
            // Act && Assert
            Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await _userService.GetByEmailAsync(It.IsAny<string>());
            });

        }
        
        [TestMethod]
        public async Task GetByPhoneNumberAsync_ValidPhoneNumber_ReturnsUserResponse()
        {
            // Arrange
            var phoneNumber = "1234567890";
            var expectedUser = new User
            {
                FirstName = "Jane",
                LastName = "Smith",
                UserName = "janesmith",
                Email = "jane@example.com",
                AverageRating = (Decimal)4.2
            };
            _mockUserRepository.Setup(repo => repo.GetByPhoneNumberAsync(phoneNumber)).ReturnsAsync(expectedUser);

            // Act
            var result = await _userService.GetByPhoneNumberAsync(phoneNumber);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUser.FirstName, result.FirstName);
            Assert.AreEqual(expectedUser.LastName, result.LastName);
            Assert.AreEqual(expectedUser.UserName, result.Username);
            Assert.AreEqual(expectedUser.Email, result.Email);
            Assert.AreEqual(expectedUser.AverageRating, result.AverageRating);
        }
        [TestMethod]
        public async Task GetByPhoneNumberAsync_WhenUserNotFound_ThrowsAsync()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUser = new User
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Email = email,
                AverageRating = 4
            };
            _mockUserRepository.Setup(repo => repo.GetByPhoneNumberAsync(email))
                .ThrowsAsync(new EntityNotFoundException("User not found!"));

            // Act && Assert
            Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await _userService.GetByPhoneNumberAsync(It.IsAny<string>());
            });

        }
    }
}
