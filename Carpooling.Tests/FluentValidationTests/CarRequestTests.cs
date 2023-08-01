using Carpooling.BusinessLayer.Dto_s.Requests;
using Carpooling.Fluent_Validation;
using FluentValidation.TestHelper;

namespace Carpooling.Tests.FluentValidationTests
{
    [TestClass]
    public class CarRequestTests
    {
        private CarRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new CarRequestValidator();
        }

        [TestMethod]

        public void CarRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "A3",
                Brand = "Audi",
                AvailableSeats = 4,
                TotalSeats = 4,
                Registration = "CB1515CB"
            };

            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenRegistrationEmpty()
        {
            string registration = null;

            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "A3",
                Brand = "Audi",
                AvailableSeats = 4,
                TotalSeats = 4,
                Registration = registration
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Registration);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property cannot be empty"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenBrandEmpty()
        {
            string brand = null;

            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "A3",
                Brand = brand,
                AvailableSeats = 4,
                TotalSeats = 4,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Brand);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property cannot be empty"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenModelEmpty()
        {
            string model2 = null;

            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = model2,
                Brand = "A3",
                AvailableSeats = 4,
                TotalSeats = 4,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Model);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property cannot be empty"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenColorEmpty()
        {
            string color = null;

            //Arrange
            var model = new CarDTO()
            {
                Color = color,
                Model = "Audi",
                Brand = "A3",
                AvailableSeats = 4,
                TotalSeats = 4,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Color);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property cannot be empty"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenTotalSeatsEmpty()
        {
            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "Audi",
                Brand = "A3",
                AvailableSeats = 4,
                TotalSeats = 0,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.TotalSeats);

            var msg = result.Errors;

            Assert.AreEqual(2, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Property cannot be empty"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenTotalSeatsNegative()
        {
            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "Audi",
                Brand = "A3",
                AvailableSeats = 4,
                TotalSeats = -1,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.TotalSeats);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Seats cannot be negative"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenTotalSeatsGreater()
        {
            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "Audi",
                Brand = "A3",
                AvailableSeats = 4,
                TotalSeats = 6,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.TotalSeats);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Seats cannot be more than 5"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenAvailableSeatsNegative()
        {
            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "Audi",
                Brand = "A3",
                AvailableSeats = -1,
                TotalSeats = 5,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.AvailableSeats);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Seats cannot be negative"));
        }

        [TestMethod]
        public void CarRequest_ShouldThrow_WhenAvailableSeatsGreater()
        {
            //Arrange
            var model = new CarDTO()
            {
                Color = "Black",
                Model = "Audi",
                Brand = "A3",
                AvailableSeats = 5,
                TotalSeats = 5,
                Registration = "CB1515CB"
            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.AvailableSeats);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Seats cannot be more than 4"));
        }
    }
}
