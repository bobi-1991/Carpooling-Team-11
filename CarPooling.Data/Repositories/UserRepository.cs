using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
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

        public UserRepository(CarPoolingDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        public async Task<User> UpdateAsync(string id, User user)
        {
            throw new NotImplementedException();
            //User userToUpdate = await GetByIdAsync(id);

            //if (await this.dbContext.Users.AnyAsync(x => x.Email == user.Email))
            //{
            //    throw new DublicateEntityException($"Email: {user.Email} is already exist");
            //}

            //var updatedUser =  userToUpdate.Update(user);

            //dbContext.Update(updatedUser);
            //dbContext.SaveChanges();

            //return updatedUser;
        }

        public async Task<string> BanUser(User userToBeBanned)
        {
            userToBeBanned.IsBlocked = true;
            dbContext.SaveChanges();
            return "User successfully banned";
        }
    }
}
