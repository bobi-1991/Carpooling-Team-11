using Carpooling.Fluent_Validation;
using Carpooling.Service.Dto_s.Requests;
using FluentValidation.TestHelper;

namespace Carpooling.Tests.FluentValidationTests
{
    [TestClass]
    public class TripRequestTests
    {
        private TripRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new TripRequestValidator();
        }

        [TestMethod]

        public void TripRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new TripRequestRequest()
            {
                DriverId = "123123123",
                TravelId = 1,
                AuthorId = "123123123"
            };


            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]

        public void TripRequest_ShouldThrow_WhenDriverIdEmpty()
        {
            string driverId = null;

            //Arrange
            var model = new TripRequestRequest()
            {
                DriverId = driverId,
                TravelId = 1,
                AuthorId = "123123123"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.DriverId);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Required Id"));
        }

        [TestMethod]

        public void TripRequest_ShouldThrow_WhenAuthorIdEmpty()
        {
            string authorId = null;

            //Arrange
            var model = new TripRequestRequest()
            {
                DriverId = "123123123",
                TravelId = 1,
                AuthorId = authorId
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.AuthorId);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Required Id"));
        }

        [TestMethod]

        public void TripRequest_ShouldThrow_WhenTravelIdEmpty()
        {
            int travelId = 0;

            //Arrange
            var model = new TripRequestRequest()
            {
                DriverId = "123123123",
                TravelId = travelId,
                AuthorId = "123123123"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.TravelId);

            var msg = result.Errors;

            Assert.AreEqual(2, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Required Id"));
        }

        [TestMethod]

        public void TripRequest_ShouldThrow_WhenTravelIdNegative()
        {
            int travelId = -1;

            //Arrange
            var model = new TripRequestRequest()
            {
                DriverId = "123123123",
                TravelId = travelId,
                AuthorId = "123123123"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.TravelId);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Cannot be negative"));
        }
    }
}
