using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface ICityRepository
    {
        List<City> GetAll();
        City GetById(int id);
        City GetByName(string name);
        City Create(City city);
        City Update(int id, City city);
        City Delete(int id);
    }
}
