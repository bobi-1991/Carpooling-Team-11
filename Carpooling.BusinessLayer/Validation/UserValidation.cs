using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Helpers;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Validation
{
    public class UserValidation : IUserValidation
    {
        private readonly UserManager<User> userManager;
        private readonly IIdentityHelper identityHelper;
        private readonly IUserRepository userRepository;

        public UserValidation(UserManager<User> userManager, IIdentityHelper identityHelper, IUserRepository userRepository)
        {
            this.userManager = userManager;
            this.identityHelper = identityHelper;
            this.userRepository = userRepository;
        }

        public async Task<bool> ValidateUserLoggedAndAdmin(User loggedUser, string id)
        {
            var role = await identityHelper.GetRole(loggedUser);

            if (!loggedUser.Id.Equals(id) && role != "Administrator")
            {
                throw new EntityUnauthorizatedException("I'm sorry, but you cannot change other user's personal data.");
            }

            return true;
        }

        public async Task<bool> ValidateIfUsernameExist(string username)
        {
            var doesExists = await this.userRepository.DoesExist(username);

            if (!doesExists)
            {
                throw new EntityNotFoundException($"User with username: {username} not found.");
            }

            return true;
        }

        public async Task<bool> ValidateLoggedUserIsAdmin(User user)
        {
            var role = await this.identityHelper.GetRole(user);

            if (role != "Administrator")
            {
                throw new EntityUnauthorizatedException("I am sorry, you are not an administrator to perform this operation");
            }

            return true;
        }

        public async Task<bool> ValidateUserAlreadyBanned(User userToBeBanned)
        {
            if (userToBeBanned.IsBlocked)
            {
                throw new ArgumentException("The user you are trying to ban is already banned");
            }

            return true;
        }
        public async Task<bool> ValidateUserNotBanned(User userToBeUnBanned)
        {
            if (!userToBeUnBanned.IsBlocked)
            {
                throw new ArgumentException("The user you are trying to UnBan is not banned");
            }

            return true;
        }

    }
}
