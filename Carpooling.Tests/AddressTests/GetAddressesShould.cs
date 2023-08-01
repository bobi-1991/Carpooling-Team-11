using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.Tests.AddressTests
{
    [TestClass]
    public class GetAddressesShould
    {
        [TestMethod]
        public async Task GetAllAsync_NoAddresses_ReturnsEmptyList()
        {
            // Arrange
            var addressesData = new List<Address>();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new EmptyListException("No addresses yet!"));

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act & Assert

            Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await addressService.GetAllAsync();
            });


        }
        [TestMethod]
        public async Task GetAllAsync_AddressesExist_ReturnsAllAddresses()
        {
            // Arrange
            var addressesData = TestHelpers.TestHelper.GetTestAddresses();

            var addressRepositoryMock = new Mock<IAddressRepository>();
            addressRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(addressesData);

            var addressService = new AddressService(addressRepositoryMock.Object);

            // Act
            var result = await addressService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }
    }
}
