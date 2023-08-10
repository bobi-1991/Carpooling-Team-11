
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);   
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByPhoneNumberAsync(string phoneNumber);
        Task<bool> DoesExist(string username);
        Task<IEnumerable<Travel>> TravelHistoryAsync(string userId);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(string id, User user);
        Task<string> DeleteAsync(string id);
        Task<string> BanUser(User userToBeBanned);
        Task<string> UnBanUser(User userToBeUnBanned);
        Task<IEnumerable<User>> GetTopTravelOrganizers(IEnumerable<User> users, int count);
        Task<IEnumerable<User>> GetTopPassengers(IEnumerable<User> users, int count);
        Task ConvertToAdministrator(string id);

        //   Task<IEnumerable<Car>> SeeAllCarsAsync(int userId);
        //  Task<IEnumerable<Feedback>> SeeAllPassengerFeedbacksAsync(int userId);
        // Task<IEnumerable<Feedback>> SeeAllDriverFeedbacksAsync(int userId);

        // Request TripRequests:

        //  Task<IEnumerable<TripRequest>> SeeAllPassengerTripRequestsAsync(int userId);
        //  Task<IEnumerable<TripRequest>> SeeAllDriverTripRequestsAsync(int userId);
        //  Task<TripRequest> CreateRequestAsync(int userId, int recipientId, int travelId);
        // Task<TripRequest> EditRequestAsync(int userId, int requestId, bool answer);
        //   Task DeleteRequestAsync(int userId, int requestId);



    }
}
