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
    public interface ITripRequestService
    {
        Task<IEnumerable<TripRequestResponse>> GetAllAsync();
        Task<IEnumerable<TripRequestResponse>> GetAllDriverRequestsAsync();
        Task<IEnumerable<TripRequestResponse>> GetAllPassengerRequestsAsync();
        Task<TripRequestResponse> GetByIdAsync(int id);
        Task<TripRequestResponse> CreateAsync(User loggedUser, TripRequestRequest tripReqeust);
        Task<string> DeleteAsync(User loggedUser, int tripRequestId);
        Task<string> EditRequestAsync(User loggedUser, int tripId, string answer);
        Task<IEnumerable<TripRequestResponse>> SeeAllHisDriverRequestsAsync(User loggedUser, string userId);
        Task<IEnumerable<TripRequestResponse>> SeeAllHisPassengerRequestsAsync(User loggedUser, string userId);
    }
}
