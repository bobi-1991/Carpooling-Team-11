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
        List<Country> GetAll();
        Country GetById(int id);
        Country Create(Country country, User user);
        Country Update(int id, Country country, User user);
        Country Delete(int id, User user);
        List<Country> FilterCountriesByName(string orderByName);
    }
}
