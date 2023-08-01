using Carpooling.BusinessLayer.Services;
using CarPooling.BusinessLayer.Services;
using CarPooling.Data.Exceptions;
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
    public class GetAddressShould
    {
        [TestMethod]
        public async Task GetByIdAsync_AddressFound_ReturnsAddress()
        {
            // Arrange
            var address = TestHelpers.TestHelper.GetTestAddressOne();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(address);

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act
            var result = await addressService.GetByIdAsync(It.IsAny<int>());

            // Assert
            Assert.AreEqual(address.Details, result.Details);
            Assert.AreEqual(address.City, result.City);
            Assert.AreEqual(address.Country.Name, result.Country.Name);
        }
        [TestMethod]
        public async Task GetByIdAsync_AddressNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new EntityNotFoundException("No such address!"));

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await addressService.GetByIdAsync(It.IsAny<int>());
            });
        }
    }
}
