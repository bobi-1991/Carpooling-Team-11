using Carpooling.Fluent_Validation;
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
    public class TravelRequestTests
    {
        private TravelRequestValidator validator;

        [TestInitialize]

        public void Initialize()
        {
            validator = new TravelRequestValidator();
        }

        //public TravelRequestValidator()
        //{
        //    RuleFor(x => x.CarId)
        //        .NotEmpty()
        //        .WithMessage("Required Id")
        //        .GreaterThan(0)
        //        .WithMessage("Cannot be negative");

        //    RuleFor(x => x.StartLocationId)
        //        .NotEmpty()
        //        .WithMessage("Required Id")
        //        .GreaterThan(0)
        //        .WithMessage("Cannot be negative");

        //    RuleFor(x => x.DestionationId)
        //        .NotEmpty()
        //        .WithMessage("Required Id")
        //        .GreaterThan(0)
        //        .WithMessage("Cannot be negative");

        //    RuleFor(x => x.DriverId)
        //        .NotEmpty()
        //        .WithMessage("Required Id");
        //}

        [TestMethod]

        public void TravelRequest_ShouldReturn_WhenAllValid()
        {
            //Arrange
            var model = new TravelRequest()
            {
                CarId = 1,
                StartLocationId = 1,
                DestionationId = 1,
                DriverId = "123123123"
            };


            //Act
            var result = validator.TestValidate(model);

            //Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestMethod]
        public void TravelRequest_ShouldThrow_WhenDriverIdEmpty()
        {
            string driverId = null;

            //Arrange
            var model = new TravelRequest()
            {
                CarId = 1,
                StartLocationId = 1,
                DestionationId = 1,
                DriverId = driverId

            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.DriverId);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Required Id"));
        }

        [TestMethod]
        public void TravelRequest_ShouldThrow_WhenDestinationIdEmpty()
        {
            //Arrange
            var model = new TravelRequest()
            {
                CarId = 1,
                StartLocationId = 1,
                DestionationId = 0,
                DriverId = "123123123"

            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.DestionationId);

            var msg = result.Errors;

            Assert.AreEqual(2, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Required Id"));
        }

        [TestMethod]
        public void TravelRequest_ShouldThrow_WhenDestinationIdNegative()
        {
            //Arrange
            var model = new TravelRequest()
            {
                CarId = 1,
                StartLocationId = 1,
                DestionationId = -1,
                DriverId = "123123123"

            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.DestionationId);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Cannot be negative"));
        }

        [TestMethod]
        public void TravelRequest_ShouldThrow_WhenStartLocationIdEmpty()
        {
            //Arrange
            var model = new TravelRequest()
            {
                CarId = 1,
                StartLocationId = 0,
                DestionationId = 1,
                DriverId = "123123123"

            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.StartLocationId);

            var msg = result.Errors;

            Assert.AreEqual(2, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Required Id"));
        }

        [TestMethod]
        public void TravelRequest_ShouldThrow_WhenStartLocationIdNegative()
        {
            //Arrange
            var model = new TravelRequest()
            {
                CarId = 1,
                StartLocationId = -1,
                DestionationId = 1,
                DriverId = "123123123"

            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.StartLocationId);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Cannot be negative"));
        }

        [TestMethod]
        public void TravelRequest_ShouldThrow_WhenCarIdEmpty()
        {
            //Arrange
            var model = new TravelRequest()
            {
                CarId = 0,
                StartLocationId = 1,
                DestionationId = 1,
                DriverId = "123123123"

            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.CarId);

            var msg = result.Errors;

            Assert.AreEqual(2, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Required Id"));
        }

        [TestMethod]
        public void TravelRequest_ShouldThrow_WhenCarIdNegative()
        {
            //Arrange
            var model = new TravelRequest()
            {
                CarId = -1,
                StartLocationId = 1,
                DestionationId = 1,
                DriverId = "123123123"

            };

            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.CarId);

            var msg = result.Errors;

            Assert.AreEqual(1, msg.Count);
            Assert.IsTrue(msg.Any(x => x.ErrorMessage == "Cannot be negative"));
        }
    }
}
