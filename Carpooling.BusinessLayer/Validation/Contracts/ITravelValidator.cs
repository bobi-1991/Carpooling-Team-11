using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Validation.Contracts
{
    public interface ITravelValidator
    {
        Task<bool> ValidateIsNewTravelPossible(string driverId, DateTime currentDeparture,DateTime currentArrival);
        Task<bool> ValidateIsLoggedUserAreDriver(User loggedUser,string driverId);
        Task<bool> CheckIsUpdateDataAreValid(Travel travelToUpdate, TravelUpdateDto travelDataForUpdate);

    }
}
