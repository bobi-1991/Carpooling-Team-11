using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Validation.Contracts
{
    public interface IUserValidation
    {
        Task<bool> ValidateUserLoggedAndAdmin(User loggedUser, string id);
        Task<bool> ValidateIfUsernameExist(string username);
        Task<bool> ValidateLoggedUserIsAdmin(User user);
        Task<bool> ValidateUserAlreadyBanned(User userToBeBanned);
        Task<bool> ValidateUserNotBanned(User userToBeUnBanned);



    }
}
