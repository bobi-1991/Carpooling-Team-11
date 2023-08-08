using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Task<string> DeleteAsync(int travelId);
        Task<IEnumerable<Travel>> FilterTravelsAndSortAsync(string sortBy);
        Task AddUserToTravelAsync(int travelId, string passengerId);
        Task RemoveUserToTravelAsync(int travelId, string passengerId);
        Task <PaginatedList<Travel>> FilterByAsync (TravelQueryParameters filter);
        Task<string> SetTravelToIsCompleteAsync(Travel travel);


        //Task<TravelDTO> AddStopToTravelAsync();
    }
}
