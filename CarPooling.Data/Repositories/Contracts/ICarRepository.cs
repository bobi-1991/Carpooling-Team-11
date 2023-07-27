using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface ICarRepository
    {
        List<Car> GetAll();
        Car GetById(int id);
        Car GetByBrand(string brandName);
        Car Create(Car car);
        Car Update(int id, Car car);
        Car Delete(int id);
        List<Car> FilterCarsAndSort(string sortBy);
    }
}
