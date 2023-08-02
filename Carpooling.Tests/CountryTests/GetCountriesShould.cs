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
    public class GetCountriesShould
    {
        [TestMethod]
        public async Task GetAllAsync_NoCountries_ThrowsEmptyListException()
        {
            // Arrange

            var country = TestHelpers.TestHelper.GetTestCountryOne();

            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(new EmptyListException("No countries yet!"));

            var countryService = new CountryService(repositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<EmptyListException>(async () =>
            {
                await countryService.GetAllAsync();
            });
        }

        [TestMethod]
        public async Task GetAllAsync_CountriesExist_ReturnsNonDeletedCountries()
        {
            // Arrange
            var countriesData = TestHelpers.TestHelper.GetTestCountries();

            var country = TestHelpers.TestHelper.GetTestCountryOne();

            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(countriesData);

            var countryService = new CountryService(repositoryMock.Object);

            // Act
            var result = await countryService.GetAllAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
        }
    }
}

