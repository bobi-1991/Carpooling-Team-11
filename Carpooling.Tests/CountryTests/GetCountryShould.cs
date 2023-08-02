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
    public class GetCountryShould
    {
        [TestMethod]
        public async Task GetByIdAsync_CountryExists_ReturnsCountry()
        {
            // Arrange
            var country = TestHelpers.TestHelper.GetTestCountryOne();

            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(country);

            var countryService = new CountryService(repositoryMock.Object);

            // Act
            var result = await countryService.GetByIdAsync(country.Id);

            // Assert
            Assert.AreEqual(country.Name, result.Name);
        }

        [TestMethod]
        public async Task GetByIdAsync_CountryNotFound_ThrowsEntityNotFoundException()
        {
            // Arrange
            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new EntityNotFoundException("No such country!"));

            var countryService = new CountryService(repositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () =>
            {
                await countryService.GetByIdAsync(It.IsAny<int>());
            });
        }
    }
}
