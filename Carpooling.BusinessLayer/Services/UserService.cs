using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IUserValidation userValidator;

        private readonly UserManager<User> _userManager;


        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            _userManager = userManager;
        }
        public Task<User> GetByUsernameAuthAsync(string username)
        {
            return this.userRepository.GetByUsernameAsync(username);
        }

        public async Task<UserResponse> RegisterAsync(UserRequest userRequest)
        {
            var user = mapper.Map<User>(userRequest);

            //await _userStore.SetUserNameAsync(user, user.Email, CancellationToken.None);
            //await _emailStore.SetEmailAsync(user, user.Email, CancellationToken.None);
            //var result = await _userManager.CreateAsync(user, user.Password);
            //await _userManager.AddToRolesAsync(user, new List<string> { "Passenger" });

            var result = await this._userManager.CreateAsync(user, userRequest.Password);
            await _userManager.AddToRoleAsync(user, "Passenger");

            return mapper.Map<UserResponse>(result);
        }

        public async Task<string> DeleteAsync(User loggedUser, string id)
        {
            await userValidator.ValidateUserLoggedAndAdmin(loggedUser, id);
            var userToDelete = await this.userRepository.GetByIdAsync(id);

            await this._userManager.DeleteAsync(userToDelete);

            return "User successfully deleted.";
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var result = await this.userRepository.GetAllAsync();

            return result.Select(x => mapper.Map<UserResponse>(x));
        }

        public async Task<UserResponse> GetByIdAsync(string id)
        {
            return this.mapper.Map<UserResponse>(await this.userRepository.GetByIdAsync(id));
        }

        public async Task<UserResponse> GetByUsernameAsync(string username)
        {
            return this.mapper.Map<UserResponse>(await this.userRepository.GetByUsernameAsync(username));
        }

        public async Task<IEnumerable<TravelResponse>> TravelHistoryAsync(User loggeduser, string userId)
        {
            await this.userValidator.ValidateUserLoggedAndAdmin(loggeduser,userId);
            var travels = await this.userRepository.TravelHistoryAsync(userId);

            return travels.Select(x => mapper.Map<TravelResponse>(x));
        }

        public async Task<UserResponse> UpdateAsync(User loggedUser, string id, UserUpdateDto userUpdateDto)
        {
            await userValidator.ValidateUserLoggedAndAdmin(loggedUser, id);

            var userData = this.mapper.Map<User>(userUpdateDto);

            var user = userRepository.UpdateAsync(id, userData);

            return this.mapper.Map<UserResponse>(user);
        }

    }
}
