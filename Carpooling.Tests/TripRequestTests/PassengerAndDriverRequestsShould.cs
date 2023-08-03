using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using CarPooling.Data.Repositories.Contracts;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.TripRequestTests
{
    [TestClass]
    public class PassengerAndDriverRequestsShould
    {
        [TestClass]
        public class TripRequestServiceTests
        {
            private Mock<ITripRequestRepository> tripRequestRepositoryMock;
            private Mock<IUserValidation> userValidationMock;
            private Mock<ITravelRepository> travelRepositoryMock;
            private Mock<IUserRepository> userRepositoryMock;
            private Mock<ITripRequestValidator> tripRequestValidatorMock;
            private TripRequestService tripRequestService;

            [TestInitialize]
            public void Initialize()
            {
                tripRequestRepositoryMock = new Mock<ITripRequestRepository>();
                userValidationMock = new Mock<IUserValidation>();
                travelRepositoryMock = new Mock<ITravelRepository>();
                userRepositoryMock = new Mock<IUserRepository>();
                tripRequestValidatorMock = new Mock<ITripRequestValidator>();

                tripRequestService = new TripRequestService(
                    tripRequestRepositoryMock.Object,
                    userValidationMock.Object,
                    travelRepositoryMock.Object,
                    userRepositoryMock.Object,
                    tripRequestValidatorMock.Object);
            }

            [TestMethod]
            public async Task SeeAllHisDriverRequestsAsync_ValidData_ReturnsTripRequestResponses()
            {
                // Arrange
                var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
                var driverId = "1";
                var tripRequests = new List<TripRequest>
                {
                    new TripRequest
                    {
                        Passenger = TestHelpers.TestHelper.GetTestUserTwo(),
                        Driver = TestHelpers.TestHelper.GetTestUserOne(),
                        Travel = new Travel
                        {
                            Id = 1,
                            StartLocation =TestHelpers.TestHelper.GetTestAddressOne(),
                            EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                            DepartureTime = DateTime.Now
                        },
                        Status = TripRequestEnum.Approved
                    },
                    new TripRequest
                    {
                        Passenger = TestHelpers.TestHelper.GetTestUserTwo(),
                        Driver = TestHelpers.TestHelper.GetTestUserOne(),
                        Travel = new Travel
                        {
                            Id = 2,
                            StartLocation =TestHelpers.TestHelper.GetTestAddressThree(),
                            EndLocation = TestHelpers.TestHelper.GetTestAddressFour(),
                            DepartureTime = DateTime.Now.AddDays(1)
                        },
                        Status = TripRequestEnum.Approved
                    }

                };

                userValidationMock.Setup(validator => validator.ValidateUserLoggedAndAdmin(loggedUser, driverId)).
                    Verifiable();
                tripRequestRepositoryMock.Setup(repo => repo.SeeAllHisDriverRequestsAsync(driverId))
                    .ReturnsAsync(tripRequests);

                // Act
                var result = await tripRequestService.SeeAllHisDriverRequestsAsync(loggedUser, driverId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count());
                Assert.IsTrue(result.Any(r => r.PassengerUsername == "testTwo@gmail.com"));
                Assert.IsTrue(result.All(r => r.DriverUsername == "testOne@gmail.com"));
            }

            [TestMethod]
            public async Task SeeAllHisPassengerRequestsAsync_ValidData_ReturnsTripRequestResponses()
            {
                // Arrange
                var loggedUser = TestHelpers.TestHelper.GetTestUserOne();
                var passengerId = "1";
                var tripRequests = new List<TripRequest>
            {
                new TripRequest
                    {
                        Driver = TestHelpers.TestHelper.GetTestUserTwo(),
                        Passenger = TestHelpers.TestHelper.GetTestUserOne(),
                        Travel = new Travel
                        {
                            Id = 1,
                            StartLocation =TestHelpers.TestHelper.GetTestAddressOne(),
                            EndLocation = TestHelpers.TestHelper.GetTestAddressTwo(),
                            DepartureTime = DateTime.Now
                        },
                        Status = TripRequestEnum.Approved
                    },
                    new TripRequest
                    {
                        Driver = TestHelpers.TestHelper.GetTestUserTwo(),
                        Passenger = TestHelpers.TestHelper.GetTestUserOne(),
                        Travel = new Travel
                        {
                            Id = 2,
                            StartLocation =TestHelpers.TestHelper.GetTestAddressThree(),
                            EndLocation = TestHelpers.TestHelper.GetTestAddressFour(),
                            DepartureTime = DateTime.Now.AddDays(1)
                        },
                        Status = TripRequestEnum.Approved
                    }
            };

                userValidationMock.Setup(validator => validator.ValidateUserLoggedAndAdmin(loggedUser, passengerId)).Verifiable();
                tripRequestRepositoryMock.Setup(repo => repo.SeeAllHisPassengerRequestsAsync(passengerId)).ReturnsAsync(tripRequests);

                // Act
                var result = await tripRequestService.SeeAllHisPassengerRequestsAsync(loggedUser, passengerId);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(2, result.Count());
                Assert.IsTrue(result.Any(r => r.DriverUsername == "testTwo@gmail.com"));
                Assert.IsTrue(result.All(r => r.PassengerUsername == "testOne@gmail.com"));
            }
        }
    }
}