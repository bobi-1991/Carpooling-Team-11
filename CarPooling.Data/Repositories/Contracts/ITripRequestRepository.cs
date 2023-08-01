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
        Task<TripRequest> UpdateTripRequestAsync(string userId, int tripRequestId, bool answer);
        Task<string> DeleteAsync(string userId, int tripRequestId);
    }
}
