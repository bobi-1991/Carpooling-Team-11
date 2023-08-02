using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.BusinessLayer.Helpers
{
    public class IdentityHelper : IIdentityHelper
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

        public async Task ChangeRole(User loggedUser, User user,string currentRole)
        {
            var role = await userManager.GetRolesAsync(user);
            var loggedUserRole = await userManager.GetRolesAsync(loggedUser);

            if (currentRole.ToLower() == "driver" || currentRole.ToLower() == "passenger")
            {
                await userManager.RemoveFromRolesAsync(user, role);
                await userManager.AddToRoleAsync(user, currentRole);
            }

            if (loggedUserRole.FirstOrDefault().ToLower() == "administrator" && currentRole.ToLower() == "administrator")
            {
                await userManager.RemoveFromRolesAsync(user, role);
                await userManager.AddToRoleAsync(user, currentRole);
            }
            
            await dbContext.SaveChangesAsync();
        }

        //Not tested yet
        public async Task<User> GetAdmin()
        {
            var adminRole = await dbContext.Roles.FirstOrDefaultAsync(role => role.Name.ToLower() == "administrator").ConfigureAwait(false);

            if (adminRole is null)
            { 
                throw new EntityNotFoundException($"Role {adminRole} is not found.");
            }

            var adminId = await dbContext.UserRoles.FirstOrDefaultAsync(role => role.RoleId == adminRole.Id).ConfigureAwait(false);

            if (string.IsNullOrEmpty(adminId.ToString()))
            {
                throw new EntityNotFoundException($"Id {adminId} is not found.");
            }

            var admin = await GetUserByID(adminId.UserId).ConfigureAwait(false);

            if (admin is null)
            {
                throw new EntityNotFoundException($"Id {admin} is not found.");
            }

            return admin;
        }

        public async Task<User> GetUserByID(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                throw new EntityNotFoundException($"Id {id} is not found.");
            }


            var user = await dbContext.Users.FindAsync(id);

            if (user is null)
            {
                throw new EntityNotFoundException($"Id {user} is not found.");
            }

            return user;

        }
    }








}

