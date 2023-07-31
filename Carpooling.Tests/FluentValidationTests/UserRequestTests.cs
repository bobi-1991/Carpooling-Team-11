using Carpooling.BusinessLayer.Validation.Fluent_Validation;
using Carpooling.Service.Dto_s.Requests;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.FluentValidationTests
{
    [TestClass]
    public class UserRequestTests
    {
        private UserRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new UserRequestValidator();
        }

        [TestMethod]

        public void UserRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new UserRequest
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Username = new string('a', 10),
                Password = "Password2@",
                Email = "abv@abv.bg",
                AddressId = 1
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void UserRequest_ShouldThrow_WhenFirstNameEmpty()
        {
            string firstName = null;

            var model = new UserRequest
            {
                FirstName = firstName,
                LastName = new string('a', 20),
                Username = new string('a', 10),
                Password = "Password2@",
                Email = "abv@abv.bg",
                AddressId = 1
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.FirstName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "First Name is required."));
        }

        [TestMethod]

        public void UserRequest_ShouldThrow_WhenLastNameEmpty()
        {
            string lastName = null;

            var model = new UserRequest
            {
                FirstName = new string('a', 20),
                LastName = lastName,
                Username = new string('a', 10),
                Password = "Password2@",
                Email = "abv@abv.bg",
                AddressId = 1
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.LastName);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Last Name is required."));
        }


        [TestMethod]

        public void UserRequest_ShouldThrow_WhenUsernameEmpty()
        {
            string username = null;

            var model = new UserRequest
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Username = username,
                Password = "Password2@",
                Email = "abv@abv.bg",
                AddressId = 1
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Username);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Username is required."));
        }

        [TestMethod]

        public void UserRequest_ShouldThrow_WhenPasswordEmpty()
        {
            string password = null;

            var model = new UserRequest
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Username = new string('a', 10),
                Password = password,
                Email = "abv@abv.bg",
                AddressId = 1
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Password is required."));
        }

        [TestMethod]

        public void UserRequest_ShouldThrow_WhenEmailEmpty()
        {
            string email = null;

            var model = new UserRequest
            {
                FirstName = new string('a', 20),
                LastName = new string('a', 20),
                Username = new string('a', 10),
                Password = "Password2@",
                Email = email,
                AddressId = 1
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Email is required."));
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(21)]

        public void UserRequest_ShouldThrow_WhenFirstNameInvalid(int count)
        {
            //Arrange
            var firstName = new string('a', count);

            var model = new UserRequest()
            {
                FirstName = firstName,
                LastName = "Mladenov",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                AddressId = 1
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

        public void UserRequest_ShouldThrow_WhenLastNameInvalid(int count)
        {
            //Arrange
            var lastName = new string('a', count);

            var model = new UserRequest()
            {
                FirstName = "Strahil",
                LastName = lastName,
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                AddressId = 1
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
        [DataRow(1)]
        [DataRow(21)]

        public void UserRequest_ShouldThrow_WhenUsernameInvalid(int count)
        {
            //Arrange
            var username = new string('a', count);

            var model = new UserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = username,
                Password = "Passw0rd@",
                Email = "abvbg@abv.bg",
                AddressId = 1
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Username);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == $"Username must be between 2 and 20 characters long. You entered {count} characters"));
        }

        [TestMethod]

        public void UserRequest_ShouldThrow_WhenPasswordInvalid()
        {
            //Arrange
            var model = new UserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = "Mladenov123",
                Password = "Pass",
                Email = "abv@abv.bg",
                AddressId = 1
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

        public void UserRequest_ShouldThrow_WhenPasswordInvalid2()
        {
            //Arrange
            var model = new UserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = "Mladenov123",
                Password = "Password123",
                Email = "abv@abv.bg",
                AddressId = 1
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

        public void UserRequest_ShouldThrow_WhenEmailInvalid()
        {
            //Arrange
            var model = new UserRequest()
            {
                FirstName = "Strahil",
                LastName = "Mladenov",
                Username = "Mladenov123",
                Password = "Passw0rd@",
                Email = "abv.bg",
                AddressId = 1
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
