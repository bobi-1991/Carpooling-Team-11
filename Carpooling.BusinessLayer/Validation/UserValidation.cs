using Carpooling.BusinessLayer.Validation.Contracts;
using CarPooling.Data.Models;
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
        public async Task<bool> ValidateUserLoggedAndAdmin(User loggedUser, string Id)
        {

            if (!loggedUser.Id.Equals(Id) && !loggedUser.IsAdmin)
            {
                throw new ValidationException("I'm sorry, but you cannot change other user's personal data.");
            }

            return true;
        }
    }
}
