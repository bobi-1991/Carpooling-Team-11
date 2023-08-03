using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Enums;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            //if (dbContext.TripRequests.Count() == 0)
            //{
            //    throw new EmptyListException("No requests yet!");
            //}
            var tripRequests = await dbContext.TripRequests
                .Where(x => !x.IsDeleted)
                .Include(x => x.Travel)
                     .ThenInclude(x => x.StartLocation)
                .Include(x => x.Passenger)
                     .ThenInclude(x => x.Address)
                .ToListAsync();

            return tripRequests;
        }

        public async Task<IEnumerable<TripRequest>> GetAllDriverRequestsAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<TripRequest>> GetAllPassengerRequestsAsync()
        {
            throw new NotImplementedException();
        }


        public async Task<TripRequest> GetByIdAsync(int id)
        {
            var tripRequest = await dbContext.TripRequests
              .Where(x => !x.IsDeleted)
              .Include(x => x.Travel)
                   .ThenInclude(x => x.StartLocation)
              .Include(x => x.Passenger)
                   .ThenInclude(x => x.Address)
              .FirstOrDefaultAsync(x => x.Id == id) ?? throw new EntityNotFoundException($"Trip request with id:{id} not found.");

            return tripRequest;

        }
        public async Task<TripRequest> CreateAsync(string driverId, string passengerId, int travelId)
        {
            var tripRequest = new TripRequest(passengerId, travelId);

            var driver = this.dbContext.Users.FirstOrDefault(x => x.Id == driverId);
            var passenger = this.dbContext.Users.FirstOrDefault(x => x.Id == passengerId);

            passenger.PassengerTripRequests.Add(tripRequest);
            driver.DriverTripRequests.Add(tripRequest);

            await this.dbContext.TripRequests.AddAsync(tripRequest);
            await dbContext.SaveChangesAsync();

            return (tripRequest);
        }
        public async Task<string> EditRequestAsync(TripRequest tripRequestToUpdate, string answer)
        {
            if (answer == "approve")
            {
                tripRequestToUpdate.Status = TripRequestEnum.Approved;
            }
            else
            { 
                tripRequestToUpdate.Status = TripRequestEnum.Declined;
            }

            this.dbContext.Update(tripRequestToUpdate);
            await this.dbContext.SaveChangesAsync();
           

            return "Status successfully changed.";
        }

        public async Task<string> DeleteAsync(int tripRequestId)
        {

            var tripRequestToDelete = await this.dbContext.TripRequests
                .FirstOrDefaultAsync(x => x.Id == tripRequestId);


            tripRequestToDelete.IsDeleted = true;
            await this.dbContext.SaveChangesAsync();

            return "Trip request successfully deleted";
        }
        public async Task<IEnumerable<TripRequest>> SeeAllHisDriverRequestsAsync(string userId)
        {
            var tripRequests = await dbContext.TripRequests
                     .Where(x => !x.IsDeleted)
                     .Include(x => x.Travel)
                     .Include(x=>x.Driver)
                     .Include(x => x.Travel)
                          .ThenInclude(x => x.StartLocation)
                     .Include(x => x.Passenger)
                          .ThenInclude(x => x.Address)
                     .Where(x=>x.DriverId == userId)
                     .ToListAsync();


            if (tripRequests == null || tripRequests.Count() == 0)
            {
                throw new EntityNotFoundException($"Driver with Id: {userId} does not have any recipient requests");
            }

            return tripRequests;
        }
        public async Task<IEnumerable<TripRequest>> SeeAllHisPassengerRequestsAsync(string passengerId)
        {
            var tripRequests = await dbContext.TripRequests
                   .Where(x => !x.IsDeleted)
                   .Include(x => x.Travel)
                   .Include(x => x.Driver)
                   .Include(x => x.Travel)
                        .ThenInclude(x => x.StartLocation)
                   .Include(x => x.Passenger)
                        .ThenInclude(x => x.Address)
                   .Where(x => x.PassengerId == passengerId)
                   .ToListAsync();


            if (tripRequests == null || tripRequests.Count() == 0)
            {
                throw new EntityNotFoundException($"Passenger with Id: {passengerId} does not have any recipient requests");
            }

            return tripRequests;
        }
    }
}
