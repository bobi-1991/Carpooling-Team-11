using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarPoolingDbContext dbContext;
        private readonly UserManager<User> userManger;


        public UserRepository(CarPoolingDbContext dbContext, UserManager<User> userManger)
        {
            this.dbContext = dbContext;
            this.userManger = userManger;
        }

        public async Task<User> CreateAsync(User user)
        {

            this.dbContext.Users.Add(user);

            await this.dbContext.SaveChangesAsync();

            return await this.dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == user.Id);
        }

        public async Task<string> DeleteAsync(string id)
        {
            var user = dbContext.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                user.IsDeleted = true;

                dbContext.SaveChanges();
            }

            return "User was successfully deleted.";
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return dbContext.Users
             .Where(x => !x.IsDeleted)
             .ToList();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = dbContext.Users
             .Where(x => !x.IsDeleted)
             .FirstOrDefault(x => x.Id == id);

            if (user is null)
            {
                throw new EntityNotFoundException($"User with id:{id} not found.");
            }

            return user;
        }

        public async Task<bool> DoesExist(string username)
        {
            return dbContext.Users
           .Where(x => !x.IsDeleted)
           .Any(x => x.UserName.Equals(username));
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = dbContext.Users
             .Where(x => !x.IsDeleted)
             .FirstOrDefault(x => x.UserName == username);

            if (user is null)
            {
                throw new EntityNotFoundException($"User with username:{username} not found.");
            }

            return user;
        }

        public async Task<IEnumerable<Travel>> TravelHistoryAsync(string userId)
        {
            var user = await GetByIdAsync(userId);

            return user.TravelHistory;
        }

        public async Task<User> UpdateAsync(string id, User user,string role)
        {
            User userToUpdate = await GetByIdAsync(id);
            var userEmail = await this.dbContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (userEmail is not null)
            {
                if (!userToUpdate.Email.Equals(userEmail.Email))
                {
                    throw new DublicateEntityException($"Email: {user.Email} is already exist");
                }
            }

            userToUpdate.FirstName = user.FirstName ?? userToUpdate.FirstName;
            userToUpdate.LastName = user.LastName ?? userToUpdate.LastName;
            userToUpdate.PasswordHash = user.PasswordHash ?? userToUpdate.PasswordHash;
            userToUpdate.Email = user.Email ?? userToUpdate.Email;


            await this.userManger.UpdateAsync(userToUpdate);

            //TODO
            //update role curently not working
            if (!string.IsNullOrEmpty(role))
            {
                if (role == "Passenger" || role == "Driver")
                {

                    await this.userManger.AddToRoleAsync(userToUpdate, role);
                }
                else
                {
                    throw new EntityNotFoundException($"Role {role} not exist in the system.");
                }
            }

            return userToUpdate;
        }

        public async Task<string> BanUser(User userToBeBanned)
        {
            userToBeBanned.IsBlocked = true;
            this.dbContext.SaveChanges();
            return "User successfully banned";
        }

        public async Task<string> UnBanUser(User userToBeUnBanned)
        {
            userToBeUnBanned.IsBlocked = false;
            this.dbContext.SaveChanges();
            return "User successfully UnBanned";
        }
    }
}
