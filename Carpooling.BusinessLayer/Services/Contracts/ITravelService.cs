using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Pagination;
using Microsoft.Extensions.Hosting;
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
        Task<IEnumerable<Travel>> GetAllTravelAsync();
        Task<TravelResponse> GetByIdAsync(int travelId);
        Task<TravelResponse> CreateTravelAsync(User loggedUser, TravelRequest travel);
        Task<TravelResponse> UpdateAsync(User loggedUser, int travelId, TravelUpdateDto travelDataToUpdate);
        Task<string> DeleteAsync(User loggedUser, int travelId);
        Task<string> DeleteMVCAsync(User loggedUser, int travelId);
        Task<IEnumerable<TravelResponse>> FilterTravelsAndSortAsync(string sortBy);
        Task<Travel> CreateTravelForMVCAsync(User loggedUser, Travel travel);
        Task<string> SetTravelToIsCompleteAsync(User loggedUser, int id);
        Task<string> SetTravelToIsCompleteMVCAsync(User loggedUser, int id);
        Task<IEnumerable<Travel>> FilterTravelsAndSortForMVCAsync(string sortBy);
    }
}
