using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface ICarService
    {
        List<Car> GetAll();
        Car GetById(int id);
        Car GetByBrand(string brandName);
        Car Create(Car car, User user);
        Car Update(int id, Car car, User user);
        Car Delete(int id, User user);
        List<Car> FilterCarsAndSort(string sortBy);
    }
}
