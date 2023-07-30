using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using Carpooling.Service.Dto_s.Requests;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarPooling.Data.Exceptions;

namespace Carpooling.BusinessLayer.Validation
{
    public class AuthValidator:IAuthValidator
    {
        private readonly IUserService userService;

        public AuthValidator(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<User> ValidateCredentialAsync(string credentials)
        {
            string[] credentialsArray = credentials.Split(':');
            string username = credentialsArray[0];
            string password = credentialsArray[1];

            try
            {
                var user = await this.userService.GetByUsernameAuthAsync(username);

                var hasher = new PasswordHasher<User>();

                var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

                if (result == PasswordVerificationResult.Success)
                {
                    return user;
                }
                else
                {
                    throw new EntityUnauthorizatedException("Invalid credentials");
                }
            }
            catch(EntityNotFoundException e)
            {
                throw new EntityUnauthorizatedException("Invalid credentials");
            }
        }


    }
}
