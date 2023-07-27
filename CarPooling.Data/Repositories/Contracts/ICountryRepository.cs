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
        List<Country> GetAll();
        Country GetById(int id);
        Country Create(Country country);
        Country Update(int id, Country country);
        Country Delete(int id);
        List<Country> FilterCountriesByName(string orderByName);    
    }
}
