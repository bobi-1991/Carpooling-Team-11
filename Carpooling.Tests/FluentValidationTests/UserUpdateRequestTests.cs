using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.Fluent_Validation;
using FluentValidation.TestHelper;

namespace Carpooling.Tests.FluentValidationTests
{
    [TestClass]
    public class UserUpdateRequestTests
    {
        private UserUpdateRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new UserUpdateRequestValidator();
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new UserUpdateDto()
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Password = "Password2@",
                Email = "abv@abv.bg",
                Role = "Driver"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenFirstNameEmpty()
        {
            string firstName = null;

            var model = new UserUpdateDto()
            {
                FirstName = firstName,
                LastName = new string('a', 20),
                Password = "Password2@",
                Email = "abv@abv.bg",
                Role = "Driver"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "First Name is required."));
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenLastNameEmpty()
        {
            string lastName = null;

            var model = new UserUpdateDto()
            {
                FirstName = "Fitness",
                LastName = lastName,
                Password = "Password2@",
                Email = "abv@abv.bg",
                Role = "Driver"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.LastName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Last Name is required."));
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenPasswordEmpty()
        {
            string password = null;

            var model = new UserUpdateDto()
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Password = password,
                Email = "abv@abv.bg",
                Role = "Driver"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Password is required."));
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenEmailEmpty()
        {
            string email = null;

            var model = new UserUpdateDto()
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Password = "Password2@",
                Email = email,
                Role = "Driver"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Email is required."));
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenRoleEmpty()
        {
            string role = null;

            var model = new UserUpdateDto()
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Password = "Password2@",
                Email = "abv@abv.bg",
                Role = role
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Role);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Role is required."));
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(21)]

        public void UserUpdateRequest_ShouldThrow_WhenFirstNameInvalid(int count)
        {
            //Arrange
            var firstName = new string('a', count);

            var model = new UserUpdateDto()
            {
                FirstName = firstName,
                LastName = "Mladenov",
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                Role = "Driver"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == $"First Name must be between 2 and 20 characters long. You entered {count} characters"));
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(21)]

        public void UserUpdateRequest_ShouldThrow_WhenLastNameInvalid(int count)
        {
            //Arrange
            var lastName = new string('a', count);

            var model = new UserUpdateDto()
            {
                FirstName = "Strahil",
                LastName = lastName,
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                Role = "Driver"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.LastName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == $"Last Name must be between 2 and 20 characters long. You entered {count} characters"));
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenPasswordInvalid()
        {
            //Arrange
            var model = new UserUpdateDto()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Password = "Pass",
                Email = "abv@abv.bg",
                Role = "Driver"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);

            var msg = result.Errors;

            Assert.AreEqual(2, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Password must be minimum 8"));
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenPasswordInvalid2()
        {
            //Arrange
            var model = new UserUpdateDto()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Password = "Password123",
                Email = "abv@abv.bg",
                Role = "Driver"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage ==
            "Password must contain at least one uppercase letter, one lowercase letter and one digit. You entered Password123"));
        }

        [TestMethod]

        public void UserUpdateRequest_ShouldThrow_WhenEmailInvalid()
        {
            //Arrange
            var model = new UserUpdateDto()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Password = "Passw0rd@",
                Email = "abv.bg",
                Role = "Driver"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Email is invalid. You entered abv.bg"));
        }
    }
}
