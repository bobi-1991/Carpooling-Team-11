using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }
        public Car Create(Car car, User user)
        {
            if (user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("Only non-banned users can create cars!");
            }
            car.Driver = user;
            car.DriverId = user.Id;

            return _carRepository.Create(car);
        }

        public Car Delete(int id, User user)
        {
            Car carToDelete = GetById(id);
            if (carToDelete.DriverId != user.Id && user.IsBlocked==true)
            {
                throw new UnauthorizedOperationException("You cannot delete the car!");
            }
            carToDelete = _carRepository.Delete(id);
            return carToDelete;
        }

        public List<Car> FilterCarsAndSort(string sortBy)
        {
            return _carRepository.FilterCarsAndSort(sortBy);
        }

        public List<Car> GetAll()
        {
            return _carRepository.GetAll();
        }

        public Car GetByBrand(string brandName)
        {
            return _carRepository.GetByBrand(brandName);
        }

        public Car GetById(int id)
        {
            return _carRepository.GetById(id);
        }

        public Car Update(int id, Car car, User user)
        {
            Car carToUpdate = GetById(id);
            if (carToUpdate.DriverId != user.Id && user.IsBlocked == true)
            {
                throw new UnauthorizedOperationException("You cannot edit the car!");
            }
            carToUpdate = _carRepository.Update(id, car);
            return carToUpdate;
        }
    }
}
