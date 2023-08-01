using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;

namespace CarPooling.BusinessLayer.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public async Task<Country> CreateAsync(Country country, User user)
        {
            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("You do not have permission to create a new country!");
            }
            return await _countryRepository.CreateAsync(country);
        }

        public async Task<Country> DeleteAsync(int id, User user)
        {
            Country countryToDelete = await GetByIdAsync(id);

            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("You do not have permission to delete this country!");
            }

            countryToDelete = await _countryRepository.DeleteAsync(id);
            return countryToDelete;
        }

        public async Task<List<Country>> FilterCountriesByNameAsync(string orderByName)
        {
            return await _countryRepository.FilterCountriesByNameAsync(orderByName);
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _countryRepository.GetAllAsync();
        }

        public async Task<Country> GetByIdAsync(int id)
        {
            return await _countryRepository.GetByIdAsync(id);
        }

        public async Task<Country> UpdateAsync(int id, Country country, User user)
        {
            Country countryToUpdate = await GetByIdAsync(id);

            if (user.IsBlocked)
            {
                throw new UnauthorizedOperationException("You do not have permission to update this country!");
            }

            countryToUpdate = await _countryRepository.UpdateAsync(id, country);
            return countryToUpdate;
        }
    }
}
