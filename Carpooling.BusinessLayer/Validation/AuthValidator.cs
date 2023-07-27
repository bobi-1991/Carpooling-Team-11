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

            var user = await this.userService.GetByUsernameAuthAsync(username);

            if (user == null)
            {
                throw new EntityUnauthorizatedException("Invalid credentials");
            }

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

        //public async Task<bool> ValidateCredentialAsync(string credentials)
        //{
        //    string[] credentialsArray = credentials.Split(':');
        //    string username = credentialsArray[0];
        //    string password = credentialsArray[1];

        //    var user = await this.userService.GetByUsernameAuthAsync(username);

        //    if (user == null) return false;

        //    var hasher = new PasswordHasher<User>();

        //    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

        //    if (result == PasswordVerificationResult.Success) return true;

        //    return false;
        //}



    }
}
