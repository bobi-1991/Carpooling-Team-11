using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface ITravelService
    {
        Task<IEnumerable<TravelResponse>> GetAllAsync();
        Task<TravelResponse> GetByIdAsync(int travelId);
        Task<TravelResponse> CreateTravelAsync(User loggedUser, TravelRequest travel);
        Task<TravelResponse> UpdateAsync(User loggedUser, int travelId, TravelUpdateDto travelDataToUpdate);
        Task<string> DeleteAsync(User loggedUser, int travelId);
      //  Task<TravelResponse> AddUserToTravelAsync(string driveId, int travelId, string passengerId);
        Task<IEnumerable<TravelResponse>> FilterTravelsAndSortAsync(string sortBy);
    }
}
