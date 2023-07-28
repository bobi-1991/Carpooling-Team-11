using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAllAsync();
        Task<Country> GetByIdAsync(int id);
        Task<Country> CreateAsync(Country country);
        Task<Country> UpdateAsync(int id, Country country);
        Task<Country> DeleteAsync(int id);
        Task<List<Country>> FilterCountriesByNameAsync(string orderByName);
    }
}
