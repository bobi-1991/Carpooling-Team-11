using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllAsync();
        Task<Country> GetByIdAsync(int id);
        Task<Country> CreateAsync(Country country, User user);
        Task<Country> UpdateAsync(int id, Country country, User user);
        Task<Country> DeleteAsync(int id, User user);
        Task<List<Country>> FilterCountriesByNameAsync(string orderByName);
    }
}
