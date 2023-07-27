using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Validation
{
    public class UserValidation:IUserValidation
    {
        private readonly UserManager<User> userManager;

        public UserValidation(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> ValidateUserLoggedAndAdmin(User loggedUser, string Id)
        {

            if (!loggedUser.Id.Equals(Id) && !loggedUser.IsAdmin)
            {
                throw new EntityUnauthorizatedException("I'm sorry, but you cannot change other user's personal data.");
            }

            return true;
        }
    }
}
