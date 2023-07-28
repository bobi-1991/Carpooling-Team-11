using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly UserManager<User> _userManager;

        public CarService(ICarRepository carRepository, UserManager<User> userManager)
        {
            _carRepository = carRepository;
            _userManager = userManager;
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
            var role = _userManager.GetRolesAsync(user).ToString();
            Car carToDelete = GetById(id);
            if (carToDelete.DriverId != user.Id && user.IsBlocked == true && role.Equals("Admin"))
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
            var role = _userManager.GetRolesAsync(user).ToString();
            Car carToUpdate = GetById(id);
            if (carToUpdate.DriverId != user.Id && user.IsBlocked == true && role.Equals("Admin"))
            {
                throw new UnauthorizedOperationException("You cannot edit the car!");
            }
            carToUpdate = _carRepository.Update(id, car);
            return carToUpdate;
        }
    }
}
