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
    public class UpdateAddressShould
    {
        [TestMethod]
        public async Task UpdateAsync_UserIsNotBlocked_ReturnsUpdatedAddress()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var address = TestHelpers.TestHelper.GetTestAddressOne();
            var updatedAddress = new Address
            {
                City = "Burgas",
                Details = "Centralna Gara",
                Country = new Country { Name = "Bulgaria" }
            };

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(address);
            addressRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), updatedAddress))
                .ReturnsAsync(updatedAddress);

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act
            var result = await addressService.UpdateAsync(It.IsAny<int>(), user, updatedAddress);

            // Assert
            Assert.AreEqual(updatedAddress, result);
        }

        [TestMethod]
        public async Task UpdateAsync_UserIsBlocked_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserFourBlocked();
            var address = TestHelpers.TestHelper.GetTestAddressFive();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(address);

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await addressService.UpdateAsync(It.IsAny<int>(), user, address);
            });
        }
    }
}

