using CarPooling.BusinessLayer.Services;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Moq;
using Carpooling.Tests.TestHelpers;
using CarPooling.Data.Repositories;
using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;
using NuGet.Packaging.Signing;

namespace Carpooling.Tests.AddressTests
{
    [TestClass]
    public class CreateCountryShould
    {
        [TestMethod]
        public async Task CreateAsync_NewCountry_Succeeds()
        {
            // Arrange
            Country country = TestHelper.GetTestCountryOne();
            var user = TestHelper.GetTestUserOne();
            var repositoryMock = new Mock<ICountryRepository>();

            repositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new EntityNotFoundException("No country!"));
            repositoryMock
              .Setup(repo => repo.CreateAsync(It.IsAny<Country>()))
              .ReturnsAsync(country);

            var sut = new CountryService(repositoryMock.Object);
            // Act
            var result = await sut.CreateAsync(country, user);

            // Assert
            Assert.AreEqual(country, result);
        }

        [TestMethod]
        public async Task CreateAsync_ReactivateDeletedCountry_Success()
        {
            // Arrange
            var country = TestHelper.GetTestCountryThreeSoftDeleted();
            var user = TestHelper.GetTestUserOne();
            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                       .ThrowsAsync(new EntityNotFoundException("No country!"));
            repositoryMock
                   .Setup(repo => repo.CreateAsync(It.IsAny<Country>()))
                   .ReturnsAsync(country);
            var sut = new CountryService(repositoryMock.Object);

            // Act
            var result = await sut.CreateAsync(country, user);

            // Assert
            repositoryMock.Verify(r=>r.CreateAsync(country), Times.Once);
        }
        [TestMethod]
        public void CreateAsync_DuplicateCountry_ThrowsDuplicateEntityException()
        {
            // Arrange
            var country = TestHelper.GetTestCountryTwo();
            var user = TestHelper.GetTestUserOne();
            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(c => c.GetByIdAsync(It.IsAny<int>()))
                      .ThrowsAsync(new EntityNotFoundException("No country!"));
            repositoryMock
                   .Setup(repo => repo.CreateAsync(It.IsAny<Country>()))
                   .ThrowsAsync(new DublicateEntityException("Country already exists!"));
            var sut = new CountryService(repositoryMock.Object);

            // Act & Assert
            Assert.ThrowsExceptionAsync<DuplicateEntityException>(async () => await sut.CreateAsync(country, user));
        }
    }
}
