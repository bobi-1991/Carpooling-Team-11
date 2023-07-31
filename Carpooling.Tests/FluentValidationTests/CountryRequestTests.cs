using Carpooling.BusinessLayer.Dto_s.Requests;
using Carpooling.Fluent_Validation;
using FluentValidation.TestHelper;

namespace Carpooling.Tests.FluentValidationTests
{
    [TestClass]
    public class CountryRequestTests
    {
        private CountryRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new CountryRequestValidator();
        }

        [TestMethod]

        public void CountryRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new CountryDTO()
            {
                Country = "Bulgaria"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void CountryRequest_ShouldThrow_WhenCountryEmpty()
        {
            string country = null;

            //Arrange
            var model = new CountryDTO()
            {
                Country = country
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Country);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Country cannot be empty"));
        }
    }
}
