using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.AddressTests
{
    [TestClass]
    public class CreateAddressShould
    {
        [TestMethod]
        public async Task CreateAsync_UserIsBlocked_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserFourBlocked();
            var address = TestHelpers.TestHelper.GetTestAddressOne();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await addressService.CreateAsync(address, user);
            });
        }
        [TestMethod]
        public async Task CreateAsync_UserIsNotBlocked_ReturnsCreatedAddress()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var address = TestHelpers.TestHelper.GetTestAddressOne();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.CreateAsync(address))
                .ReturnsAsync(address);

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act
            var result = await addressService.CreateAsync(address, user);

            // Assert
            Assert.AreEqual(address, result);
        }
    }
}
