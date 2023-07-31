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
    }
}
