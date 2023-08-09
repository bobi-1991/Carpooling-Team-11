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

        public async Task<Car> CreateAsync(Car car, User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Driver"))
            {
                await _userManager.AddToRoleAsync(user, "Driver");
            }
            var updatedRoles = await _userManager.GetRolesAsync(user);
            if (user.IsBlocked && !updatedRoles.Contains("Driver") && !updatedRoles.Contains("Administrator"))
            {
                throw new UnauthorizedOperationException("Only non-banned users with role driver can create cars!");
            }
            car.Driver = user;
            car.DriverId = user.Id;

            return await _carRepository.CreateAsync(car);
        }

        public async Task<Car> DeleteAsync(int id, User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            Car carToDelete = await GetByIdAsync(id);

            if (carToDelete.DriverId != user.Id && user.IsBlocked && !roles.Contains("Administrator"))
            {
                throw new UnauthorizedOperationException("You cannot delete the car!");
            }

            carToDelete = await _carRepository.DeleteAsync(id);
            return carToDelete;
        }

        public async Task<List<Car>> FilterCarsAndSortAsync(string sortBy)
        {
            return await _carRepository.FilterCarsAndSortAsync(sortBy);
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _carRepository.GetAllAsync();
        }

        public async Task<Car> GetByBrandModelAndRegistrationAsync(string brandName, string model, string registration)
        {
            return await _carRepository.GetByBrandModelAndRegistrationAsync(brandName, model, registration);
        }

        public async Task<Car> GetByIdAsync(int id)
        {
            return await _carRepository.GetByIdAsync(id);
        }

        public async Task<Car> UpdateAsync(int id, Car car, User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            Car carToUpdate = await GetByIdAsync(id);

            if (carToUpdate.DriverId != user.Id && user.IsBlocked == true && !roles.Contains("Administrator"))
            {
                throw new UnauthorizedOperationException("You cannot edit the car!");
            }

            carToUpdate = await _carRepository.UpdateAsync(id, car);
            return carToUpdate;
        }
    }
}