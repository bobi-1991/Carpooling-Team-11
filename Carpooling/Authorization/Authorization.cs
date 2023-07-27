using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Service.Dto_s.Requests;
using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;

namespace Carpooling.Authorization
{
    public class Authorization : IAuthorization
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public Authorization(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<UserRequest> ValidateCredentialAsync(string credentials)
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

            if (result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                return mapper.Map<UserRequest>(user);
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

        //    string encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

        //    try
        //    {
        //        User user = await this.userService.GetByUsernameAuthAsync(username);
        //        if (user.Password == encodedPassword)
        //        {
        //            return user;
        //        }
        //        throw new EntityUnauthorizatedException("Invalid credentials");
        //    }
        //    catch (EntityNotFoundException)
        //    {
        //        throw new EntityUnauthorizatedException("Invalid username!");
        //    }
        //}

    }
}
