using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface ITravelRepository
    {
        Task<Travel> CreateTravelAsync(Travel travel);
        Task<IEnumerable<Travel>> GetAllAsync();
        Task<Travel> GetByIdAsync(int travelId);
        Task<Travel> UpdateAsync(int travelId, Travel travel);
        Task<Travel> DeleteAsync(int travelId);
        Task<Travel> AddUserToTravelAsync(string driveId, int travelId, string passengerId);

        //Task<TravelDTO> AddStopToTravelAsync();
    }
}
