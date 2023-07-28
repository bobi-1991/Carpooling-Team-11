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
        Task<List<Car>> GetAllAsync();
        Task<Car> GetByIdAsync(int id);
        Task<Car> GetByBrandAsync(string brandName);
        Task<Car> CreateAsync(Car car, User user);
        Task<Car> UpdateAsync(int id, Car car, User user);
        Task<Car> DeleteAsync(int id, User user);
        Task<List<Car>> FilterCarsAndSortAsync(string sortBy);
    }
}
