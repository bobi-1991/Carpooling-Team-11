using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Helpers;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Validation
{
    public class TravelValidator : ITravelValidator
    {
        private readonly IUserRepository userRepository;
        private readonly IIdentityHelper identityHelper;
        private readonly ITravelRepository travelRepository;
        private readonly CarPoolingDbContext dbContext;
        private readonly IAddressRepository addressRepository;
        private readonly ICarRepository carRepository;
        private readonly UserManager<User> userManager;


        public TravelValidator(IUserRepository userRepository, 
            IIdentityHelper identityHelper, ITravelRepository travelRepository, 
            CarPoolingDbContext dbContext, IAddressRepository addressRepository, 
            ICarRepository carRepository, UserManager<User> userManager)
        {
            this.userRepository = userRepository;
            this.identityHelper = identityHelper;
            this.travelRepository = travelRepository;
            this.dbContext = dbContext;
            this.addressRepository = addressRepository;
            this.carRepository = carRepository;
            this.userManager = userManager;
        }

        public async Task<bool> ValidateIsNewTravelPossible(string driveId, DateTime currentDeparture, DateTime currentArrival)
        {
            var travels = await this.travelRepository.GetAllAsync();

            //   var travelsHistoryOfTheDriver = travels.Where(x => x.DriverId == driveId);
            var activeTravels = await this.dbContext.Travels
                  //.Include(x => x.DepartureTime)
                  //.Include(x => x.ArrivalTime)
                  .Include(x => x.Car)
                  .Where(x => x.DriverId == driveId && !x.IsDeleted && x.IsCompleted == false)
                  .ToListAsync();

            //var activeTravels = travels.Where(x => x.DriverId == driveId).Where(x => !x.IsDeleted && x.IsCompleted == false);

            if (activeTravels.Any(t => t.DepartureTime <= currentArrival && t.ArrivalTime >= currentDeparture))
            {
                return false;
            }

            return true;
        }
        public async Task<bool> ValidateIsLoggedUserAreDriver(User loggedUser)
        {
            var roles = userManager.GetRolesAsync(loggedUser);

            if (!roles.Result.Contains("Administrator") && !roles.Result.Contains("Driver"))
            {
                throw new EntityUnauthorizatedException("I'm sorry, but you cannot create a travel because your role doesn't match the required role for it.");
            }

            return true;
        }
        public async Task<bool> CheckIsUpdateDataAreValid(Travel travelToUpdate, TravelUpdateDto travelDataForUpdate)
        {
            var currentDepartureTime = travelDataForUpdate.DepartureTime;
            var currentArrivalTime = travelDataForUpdate.ArrivalTime;

            if (!await this.ValidateIsNewTravelPossible(travelToUpdate.DriverId, currentDepartureTime, currentArrivalTime))
            {
                return false;
            }

            try
            {
                _ = await this.addressRepository.GetByIdAsync(travelDataForUpdate.StartLocationId);
                _ = await this.addressRepository.GetByIdAsync(travelDataForUpdate.DestionationId);
                _ = await this.carRepository.GetByIdAsync(travelDataForUpdate.CarId);
            }
            catch (EntityNotFoundException)
            {
                return false;
            }

            if (travelDataForUpdate.AvailableSeats < 0 || travelDataForUpdate.AvailableSeats > 4)
            {
                return false;
            }

            return true;

        }

    }
}
