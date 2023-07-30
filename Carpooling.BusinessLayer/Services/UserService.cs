using AutoMapper;
using Carpooling.BusinessLayer.Dto_s.AdminModels;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.BusinessLayer.Helpers;
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
        private readonly CarPoolingDbContext dbContext;
        private readonly IdentityHelper identityHelper;


        private readonly UserManager<User> _userManager;


        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, CarPoolingDbContext dbContext, IUserValidation userValidator, IdentityHelper identityHelper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            _userManager = userManager;
            this.dbContext = dbContext;
            this.userValidator = userValidator;
            this.identityHelper = identityHelper;
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

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Passenger");
            }
            else
            {
                var error = result.Errors.FirstOrDefault();
                throw new DublicateEntityException(error.Description);
            }

            UserResponse userResponse = new UserResponse(
                userRequest.FirstName,
                userRequest.LastName,
                userRequest.Username,
                userRequest.Email,
                0);

            return userResponse;
            // return mapper.Map<UserResponse>(result);
        }

        public async Task<string> DeleteAsync(User loggedUser, string id)
        {
            _ = await userValidator.ValidateUserLoggedAndAdmin(loggedUser, id);
            var userToDelete = await this.userRepository.GetByIdAsync(id);

            await this._userManager.DeleteAsync(userToDelete);

            return "User successfully deleted.";
        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var result = await this.userRepository.GetAllAsync();

            return result.Select(x => new UserResponse(
                x.FirstName,
                x.LastName,
                x.UserName,
                x.Email,
                x.AverageRating));

            //return result.Select(x => mapper.Map<UserResponse>(x));
        }

        public async Task<UserResponse> GetByIdAsync(string id)
        {
            var user = await this.userRepository.GetByIdAsync(id);

            return new UserResponse(
             user.FirstName,
             user.LastName,
             user.UserName,
             user.Email,
             user.AverageRating);

            // return this.mapper.Map<UserResponse>(await this.userRepository.GetByIdAsync(id));
        }

        public async Task<UserResponse> GetByUsernameAsync(string username)
        {
            var user = await this.userRepository.GetByUsernameAsync(username);

            return new UserResponse(
               user.FirstName,
               user.LastName,
               user.UserName,
               user.Email,
               user.AverageRating);

            // return this.mapper.Map<UserResponse>(await this.userRepository.GetByUsernameAsync(username));
        }

        public async Task<IEnumerable<TravelResponse>> TravelHistoryAsync(User loggeduser, string userId)
        {
            await this.userValidator.ValidateUserLoggedAndAdmin(loggeduser, userId);
            var travels = await this.userRepository.TravelHistoryAsync(userId);

            return travels.Select(x => mapper.Map<TravelResponse>(x));
        }

        public async Task<UserResponse> UpdateAsync(User loggedUser, string id, UserUpdateDto userUpdateDto)
        {
            await userValidator.ValidateUserLoggedAndAdmin(loggedUser, id);
            var userToUpdate = await this.userRepository.GetByIdAsync(id);

            var userDataToUpdate = new User
            {
                FirstName = userUpdateDto.FirstName ?? userToUpdate.FirstName,
                LastName = userUpdateDto.LastName ?? userToUpdate.LastName,
                Email = userUpdateDto.Email ?? userToUpdate.Email
            };

            // Ако има предоставена нова парола, хеширане и записване.
            if (!string.IsNullOrEmpty(userUpdateDto.Password))
            {
                var hashedPassword = _userManager.PasswordHasher.HashPassword(userDataToUpdate, userUpdateDto.Password);
                userDataToUpdate.PasswordHash = hashedPassword;
            }

            var updatedUser = await this.userRepository.UpdateAsync(id, userDataToUpdate);

            if (!string.IsNullOrEmpty(userUpdateDto.Role))
            {
                await this.identityHelper.ChangeRole(loggedUser, updatedUser, userUpdateDto.Role);
            }

            return new UserResponse(
            updatedUser.FirstName,
            updatedUser.LastName,
            updatedUser.UserName,
            updatedUser.Email,
            updatedUser.AverageRating);

            //await this._userManager.UpdateAsync(this.mapper.Map<User>(userUpdateDto));
            //return this.mapper.Map<UserResponse>(this.userRepository.GetByIdAsync(id));
        }
        public async Task<string> BanUser(User loggedUser, BanOrUnBanDto userToBeBanned)
        {
            await this.userValidator.ValidateIfUsernameExist(userToBeBanned.Username);
            await this.userValidator.ValidateLoggedUserIsAdmin(loggedUser);

            var userToBeBannedActual = await this.userRepository.GetByUsernameAsync(userToBeBanned.Username);

            await this.userValidator.ValidateUserAlreadyBanned(userToBeBannedActual);

            return await this.userRepository.BanUser(userToBeBannedActual);
        }
        public async Task<string> UnBanUser(User loggedUser, BanOrUnBanDto userToUnBan)
        {
            await this.userValidator.ValidateIfUsernameExist(userToUnBan.Username);
            await this.userValidator.ValidateLoggedUserIsAdmin(loggedUser);

            var userToBeUnBanned = await this.userRepository.GetByUsernameAsync(userToUnBan.Username);

            await this.userValidator.ValidateUserNotBanned(userToBeUnBanned);

            return await this.userRepository.UnBanUser(userToBeUnBanned);
        }

    }
}
