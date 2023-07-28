using CarPooling.Data.Data;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpooling.BusinessLayer.Helpers
{
    public class IdentityHelper
    {
        private UserManager<User> userManager;
        private CarPoolingDbContext dbContext;

        public IdentityHelper(UserManager<User> userManager, CarPoolingDbContext dbContext)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
        }

        public async Task<string> GetRole(User user)
        {
            var roles = await userManager.GetRolesAsync(user).ConfigureAwait(false);
            string role;

            if (roles.Count == 0)
            { 
                role = "No role";
            }
            else
            {
                role = roles[0];
            }

            return role;
        }

        //public async Task<User> GetAdmin()
        //{
        //    var adminRole = await dbContext.Roles.FirstOrDefaultAsync(role => role.Name.ToLower() == "administrator").ConfigureAwait(false);
        //    adminRole.ValidateIfNull(ExceptionMessages.RoleNull);
        //    var adminId = await dbContext.UserRoles.FirstOrDefaultAsync(role => role.RoleId == adminRole.Id).ConfigureAwait(false);
        //    adminId.ValidateIfNull(ExceptionMessages.UserRoleNull);
        //    var admin = await GetUserByID(adminId.UserId).ConfigureAwait(false);
        //    admin.ValidateIfNull(ExceptionMessages.AppUserNull);
        //    return admin;
        //}

        //public async Task<User> GetUserByID(string id) 
        //{

        //    id.ValidateIfNull(ExceptionMessages.IdNull);

        //    var user = await dbContext.Users.FindAsync(id);

        //    user.ValidateIfNull(ExceptionMessages.AppUserNull);

        //    return user;

        //}

    }
}
