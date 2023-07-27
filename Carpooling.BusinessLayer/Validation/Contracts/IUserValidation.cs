using CarPooling.Data.Models;
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
        Task<bool> ValidateUserLoggedAndAdmin(User loggedUser, string Id);

    }
}
