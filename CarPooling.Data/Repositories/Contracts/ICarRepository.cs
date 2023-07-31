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
        Task<List<Car>> GetAllAsync();
        Task<Car> GetByIdAsync(int id);
        Task<Car> GetByBrandModelAndRegistrationAsync(string brandName, string model, string registration);
        Task<Car> CreateAsync(Car car);
        Task<Car> UpdateAsync(int id, Car car);
        Task<Car> DeleteAsync(int id);
        Task<List<Car>> FilterCarsAndSortAsync(string sortBy);
    }
}
