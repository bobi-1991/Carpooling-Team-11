using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface ICityService
    {
        List<City> GetAll();
        City GetById(int id);
        City GetByName(string title);
        City Create(City city, User user);
        City Update(int id, User user, City city);
        City Delete(int id, User user);
    }
}
