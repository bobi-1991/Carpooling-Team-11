using Carpooling.BusinessLayer.Dto_s.AdminModels;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse> GetByIdAsync(string id);
        Task<User> GetUserByIdAsync(string id);
        Task<UserResponse> GetByUsernameAsync(string username);
        Task<UserResponse> GetByEmailAsync(string email);
        Task<UserResponse> GetByPhoneNumberAsync(string email);
        Task<string> DeleteUserWhenAdminAsync(string id);
        Task<User> GetByUsernameAuthAsync(string username);
        Task<IEnumerable<TravelResponse>> TravelHistoryAsync(User loggedUser, string userId);
        Task<UserResponse> RegisterAsync(UserRequest userRequest);
        Task<UserResponse> UpdateAsync(User loggedUser, string id, UserUpdateDto userUpdateDto);
        Task<string> DeleteAsync(User loggedUser, string id);
        Task<string> BanUser(User loggedUser, BanOrUnBanDto userToBeBanned);
        Task<string> UnBanUser(User loggedUser,BanOrUnBanDto userToBeUnBanned);
        Task<IEnumerable<User>> TopTravelOrganizers(int count);
        Task<IEnumerable<User>> TopPassengers(int count);
        Task ConvertToAdministrator(string id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<string> BanUserById(string id);
        Task<string> UnbanUserById(string id);
    }
}
