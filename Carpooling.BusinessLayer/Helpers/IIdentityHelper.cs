using CarPooling.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Helpers
{
    public interface IIdentityHelper
    {
        Task<string> GetRole(User user);

        Task ChangeRole(User loggedUser, User user, string currentRole);

        Task<User> GetAdmin();

        Task<User> GetUserByID(string id);
    }
}
