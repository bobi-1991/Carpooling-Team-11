using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories
{
    public class TripRequestRepository : ITripRequestRepository
    {
        private readonly CarPoolingDbContext dbContext;
        public TripRequestRepository(CarPoolingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<TripRequest>> GetAllAsync()
        {
            return await dbContext.TripRequests
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public async Task<TripRequest> GetByIdAsync(int id)
        {
            return dbContext.TripRequests
             .Where(x => !x.IsDeleted)
             .FirstOrDefault(x => x.Id == id);
        }
        public async Task<TripRequest> CreateAsync(string passengerId, string driverId, int travelId)
        {
            throw new NotImplementedException();

            //var user = GetByIdAsync(passengerId);
            //if (user == null)
            //{
            //    throw new EntityNotFoundException($"User with Id: {userId} does not exist");
            //}

            //var recipient = GetFullUserAsync(recipientId).Result;
            //if (recipient == null)
            //{
            //    throw new EntityNotFoundException($"User with Id: {userId} does not exist");
            //}

            //// User only has 1 current travel, travel history, but cant plan future travels until current is complete
            //if (recipient.TravelId != travelId)
            //{
            //    throw new EntityNotFoundException($"User with Id: {recipientId} does not have a Travel with Id: {travelId}");
            //}

            //var request = new Request(userId, recipientId, travelId);
            //request.Author = user;
            //request.Recipient = recipient;

            //user.AuthorRequests.Add(request);
            //recipient.RecipientRequests.Add(request);

            //await this.Db.Requests.AddAsync(request);

            //await Db.SaveChangesAsync();

            //return new RequestDTO(request);
        }
        public async Task<TripRequest> UpdateTripRequestAsync(string userId, int tripRequestId, bool answer)
        {
            var TripToUpdate = await this.dbContext.TripRequests
            .Include(x => x.Passenger)
            .Include(x => x.Travel)
             .ThenInclude(x=>x.Driver)
            .FirstOrDefaultAsync(x => x.Id == tripRequestId
            && x.PassengerId == userId);

            
            // In validator class?
            if (TripToUpdate == null)
            {
                throw new EntityNotFoundException($"User with Id: {userId} has not post a trip request with Id: {tripRequestId}");
            }

            if (answer == false)
            {
                TripToUpdate.Status = TripRequestEnum.Declined;
            }
            else
            {
                TripToUpdate.Status = TripRequestEnum.Approved;
            }

            await dbContext.SaveChangesAsync();
            return TripToUpdate;
        }

        public async Task<string> DeleteAsync(string userId, int tripRequestId)
        {
            var tripRequestToDelete = await this.dbContext.TripRequests
                .Include(x => x.Passenger)
                .Include(x => x.Travel)
                   .ThenInclude(x=>x.Driver)
                .FirstOrDefaultAsync(x => x.Id == tripRequestId
                && x.PassengerId == userId);

            // In validator class?
            if (tripRequestToDelete == null)
            {
                throw new EntityNotFoundException($"User with Id: {userId} has not post a trip request with Id: {tripRequestId}");
            }

            tripRequestToDelete.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();

            return "Trip request successfully deleted";
        }
    }
}
