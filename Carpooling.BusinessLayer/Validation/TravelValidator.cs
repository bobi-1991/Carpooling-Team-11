using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Helpers;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
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
        private readonly IdentityHelper identityHelper;
        private readonly ITravelRepository travelRepository;


        public TravelValidator(IUserRepository userRepository, IdentityHelper identityHelper, ITravelRepository travelRepository)
        {
            this.userRepository = userRepository;
            this.identityHelper = identityHelper;
            this.travelRepository = travelRepository;
        }

        public async Task<bool> ValidateIsNewTravelPossible(string driveId, DateTime currentDeparture, DateTime currentArrival)
        {
            var travelsHistoryOfTheDriver = await this.userRepository.TravelHistoryAsync(driveId);
            var activeTravels = travelsHistoryOfTheDriver.Where(x => x.IsCompleted == false);

            if (activeTravels.Any(t => t.DepartureTime <= currentArrival && t.ArrivalTime >= currentDeparture))
            {
                return false;
            }

            return true;
        }
       public async Task<bool> ValidateIsLoggedUserAreDriver(User loggedUser, string driverId)
        {
            var role = await identityHelper.GetRole(loggedUser);

            if (!loggedUser.Id.Equals(driverId) && role != "Administrator")
            {
                throw new EntityUnauthorizatedException("I'm sorry, but you cannot create a travel because your details not match with the driver details in travel request.");
            }

            if (role != "Driver" && role != "Administrator")
            {
                throw new EntityUnauthorizatedException("I'm sorry, but you cannot create a travel because your role doesn't match the required role for it.");
            }

            return true;
        }

    }
}
