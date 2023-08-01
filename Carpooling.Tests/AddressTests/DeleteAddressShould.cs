using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using CarPooling.BusinessLayer.Services;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.AddressTests
{
    [TestClass]
    public class DeleteAddressShould
    {

        [TestMethod]
        public async Task DeleteAsync_UserIsBlocked_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserFourBlocked();
            var address = TestHelpers.TestHelper.GetTestAddressOne();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new UnauthorizedOperationException("You cannot delete this addres!"));

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await addressService.DeleteAsync(It.IsAny<int>(), user);
            });
        }
        [TestMethod]
        public async Task DeleteAsync_UserIsNotBlocked_ReturnsDeletedAddress()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserOne();
            var address = TestHelpers.TestHelper.GetTestAddressOne();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(address);

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act
            var result = await addressService.DeleteAsync(It.IsAny<int>(), user);

            // Assert
            addressRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
