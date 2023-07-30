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
        Task<TravelResponse> CreateTravelAsync(TravelRequest travel);
        Task<TravelResponse> UpdateAsync(int travelId, Travel travel);
        Task<TravelResponse> DeleteAsync(int travelId);
        Task<TravelResponse> AddUserToTravelAsync(string driveId, int travelId, string passengerId);
    }
}
