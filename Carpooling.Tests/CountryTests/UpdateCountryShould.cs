using CarPooling.Data.Models;
using Carpooling.Tests.TestHelpers;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarPooling.Data.Repositories.Contracts;
using CarPooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Exceptions;
using Microsoft.AspNetCore.Routing;

namespace Carpooling.Tests.AddressTests
{
    [TestClass]
    public class UpdateCountryShould
    {
        [TestMethod]
        public async Task UpdateCountry_WithValidParams_ShouldSucced()
            {
            //Arrange
            string countryName = "Updated Country";
            Country countryToUpdate = TestHelper.GetTestCountryOne();
            Country updatedCountry = new Country
            {
                Name = countryName
            };

            User user = TestHelper.GetTestUserOne();
            var repositoryMock = new Mock<ICountryRepository>();

            repositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(countryToUpdate);
            repositoryMock
                .Setup(repo => repo.UpdateAsync(It.IsAny<int>(), updatedCountry))
                .ReturnsAsync(updatedCountry);


            var sut = new CountryService(repositoryMock.Object);

            //Act
            var result = await sut.UpdateAsync(It.IsAny<int>(), updatedCountry, user);

            //Assert
            Assert.AreEqual(updatedCountry, result);
        }
        [TestMethod]
        public async Task UpdateAsync_UserIsBlocked_ThrowsUnauthorizedOperationException()
        {
            // Arrange
            var user = TestHelpers.TestHelper.GetTestUserTwo();
            var country = TestHelpers.TestHelper.GetTestCountryTwo();

            var countryRepositoryMock = new Mock<ICountryRepository>();
            countryRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(country);

            var countryService = new CountryService(countryRepositoryMock.Object);

            // Act and Assert
            Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await countryService.UpdateAsync(It.IsAny<int>(), country, user);
            });
        }
    }
}
