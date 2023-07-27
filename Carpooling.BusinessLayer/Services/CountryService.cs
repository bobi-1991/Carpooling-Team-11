using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;
        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public Country Create(Country country, User user)
        {
            if (user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("You do not have permission to create new country!");
            }
            return _countryRepository.Create(country);
        }

        public Country Delete(int id, User user)
        {
            //Check if user is admin
            Country countryToDelete = GetById(id);
            if (user.IsBlocked == true)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this country!");
            }
            countryToDelete = _countryRepository.Delete(id);
            return countryToDelete;
        }

        public List<Country> FilterCountriesByName(string orderByName)
        {
            return _countryRepository.FilterCountriesByName(orderByName);
        }

        public List<Country> GetAll()
        {
            return _countryRepository.GetAll();
        }

        public Country GetById(int id)
        {
            return _countryRepository.GetById(id);
        }

        public Country Update(int id, Country country, User user)
        {
            Country countryToUpdate = GetById(id);
            if (user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("You do not have permission to update this country!");
            }
            countryToUpdate = _countryRepository.Update(id, country);
            return countryToUpdate;
        }
    }
}
