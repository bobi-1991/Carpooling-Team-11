using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface ITripRequestRepository
    {
        Task<IEnumerable<TripRequest>> GetAllAsync();
        Task<IEnumerable<TripRequest>> GetAllDriverRequestsAsync();
        Task<IEnumerable<TripRequest>> GetAllPassengerRequestsAsync();
        Task<TripRequest> GetByIdAsync(int id);
        Task<TripRequest> CreateAsync(string driverId, string pasengerId, int travelId);
        Task<string> EditRequestAsync(TripRequest tripRequestToUpdate,string answer);
        Task<string> DeleteAsync(int tripRequestId);
        Task<IEnumerable<TripRequest>> SeeAllHisDriverRequestsAsync(string userId);
        Task<IEnumerable<TripRequest>> SeeAllHisPassengerRequestsAsync(string userId);

    }
}
