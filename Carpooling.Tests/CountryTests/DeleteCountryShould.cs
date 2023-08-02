using Carpooling.BusinessLayer.Exceptions;
using CarPooling.Data.Models;
using Carpooling.Tests.TestHelpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarPooling.Data.Repositories.Contracts;
using CarPooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services;

namespace Carpooling.Tests.AddressTests
{
    [TestClass]
    public class DeleteCountryShould
    {
        [TestMethod]
        public async Task DeleteCountry_ShouldSucceed()
        {
            // Arrange
            Country country = TestHelper.GetTestCountryTwo();
            User user = TestHelper.GetTestUserTwo();
            var repositoryMock = new Mock<ICountryRepository>();
            repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(country);
                
            var sut = new CountryService(repositoryMock.Object);
            
            // Act
            var result = await sut.DeleteAsync(It.IsAny<int>(), user);

            // Assert
            repositoryMock.Verify(r=>r.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
        [TestMethod]
        public async Task DeleteCountry_ShouldThrow_WhenUserIsBlocked()
        {
            // Arrange
            Country country = TestHelper.GetTestCountryTwo();
            User user = TestHelper.GetTestUserFourBlocked();

            var repositoryMock = new Mock<ICountryRepository>();

            repositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(country);
            
            repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ThrowsAsync(new UnauthorizedOperationException("You cannot delete"));

            var sut = new CountryService(repositoryMock.Object);

            //Act && Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedOperationException>(async () =>
            {
                await sut.DeleteAsync(It.IsAny<int>(), user);
            });
        }
    }
}
