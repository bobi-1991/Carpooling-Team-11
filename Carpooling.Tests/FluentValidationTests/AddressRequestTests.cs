using Carpooling.BusinessLayer.Dto_s.Requests;
using Carpooling.Fluent_Validation;
using FluentValidation.TestHelper;

namespace Carpooling.Tests.FluentValidationTests
{
    [TestClass]
    public class AddressRequestTests
    {
        private AddressRequestValidator validator;

        [TestInitialize]

        public void Initialize() 
        {
            validator = new AddressRequestValidator();
        }

        [TestMethod]

        public void AddressRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new AddressDTO()
            {
                Country = "Bulgaria",
                Details = "Mladost 1",
                City = "Sofia"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void AddressRequest_ShouldThrow_WhenCountryEmpty()
        {
            string country = null;

            //Arrange
            var model = new AddressDTO()
            {
                Country = country,
                Details = "Mladost 1",
                City = "Sofia"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Country);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property is required"));
        }

        [TestMethod]
        public void AddressRequest_ShouldThrow_WhenDetailsEmpty()
        {
            string details = null;

            //Arrange
            var model = new AddressDTO()
            {
                Country = "Bulgaria",
                Details = details,
                City = "Sofia"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Details);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property is required"));
        }

        [TestMethod]
        public void AddressRequest_ShouldThrow_WhenCityEmpty()
        {
            string city = null;

            //Arrange
            var model = new AddressDTO()
            {
                Country = "Bulgaria",
                Details = "Mladost 1",
                City = city
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.City);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property is required"));
        }
    }
}
