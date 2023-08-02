using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Repositories
{
    public class TravelRepository : ITravelRepository
    {
        private readonly CarPoolingDbContext dbContext;
        private readonly IUserRepository userRepository;

        public TravelRepository(CarPoolingDbContext dbContext, IUserRepository userRepository)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
        }
        public async Task<IEnumerable<Travel>> GetAllAsync()
        {
            var result = await this.dbContext.Travels
                    .Include(x => x.Car)
                    .Include(x => x.StartLocation)
                    .Include(x => x.EndLocation)
                    .Where(x => x.IsCompleted == false)
                    .ToListAsync();

            return result;
        }

        public async Task<Travel> GetByIdAsync(int travelId)
        {
            var travel = await this.dbContext.Travels
               .Include(x => x.Car)
               .Include(x => x.StartLocation)
                  .ThenInclude(x=>x.Country)
               .Include(x => x.EndLocation)
                    .ThenInclude(x => x.Country)
               .Where(x => x.IsCompleted == false)
               .Where(x => !x.IsDeleted)
               .FirstOrDefaultAsync(x => x.Id == travelId);

            if (travel == null)
            {
                throw new EntityNotFoundException($"Travel with Id: {travelId} does not exist.");
            }

            return travel;
        }

        public async Task<Travel> CreateTravelAsync(Travel travel)
        {
            await this.dbContext.Travels.AddAsync(travel);
            travel.CreatedOn = DateTime.Now;
            travel.UpdatedOn = DateTime.Now;

            // await this.dbContext.SaveChangesAsync();


            var driver = await this.userRepository.GetByIdAsync(travel.DriverId);
            driver.TravelHistory.Add(travel);
            dbContext.Update(driver);

            await this.dbContext.SaveChangesAsync();
            return travel;
        }

        public async Task<string> DeleteAsync(int travelId)
        {
            var travelToDelete = await this.GetByIdAsync(travelId);

            travelToDelete.IsDeleted = true;
            travelToDelete.DeletedOn = DateTime.Now;

            await dbContext.SaveChangesAsync();

            return "Travel successfully deleted.";
        }



        public async Task<Travel> UpdateAsync(int travelId, Travel travelDataForUpdate)
        {
            var travelToUpdate = await this.GetByIdAsync(travelId);

            travelToUpdate.DriverId = travelDataForUpdate.DriverId ?? travelToUpdate.DriverId;
            travelToUpdate.DepartureTime = travelDataForUpdate.DepartureTime ?? travelToUpdate.DepartureTime;
            travelToUpdate.ArrivalTime = travelDataForUpdate.ArrivalTime ?? travelToUpdate.ArrivalTime;
            travelToUpdate.StartLocation = travelDataForUpdate.StartLocation ?? travelToUpdate.StartLocation;
            travelToUpdate.EndLocation = travelDataForUpdate.EndLocation ?? travelToUpdate.EndLocation;
            travelToUpdate.AvailableSeats = travelDataForUpdate.AvailableSeats ?? travelToUpdate.AvailableSeats;
            travelToUpdate.Car = travelDataForUpdate.Car ?? travelToUpdate.Car;

            dbContext.Update(travelToUpdate);
            await dbContext.SaveChangesAsync();

            return travelToUpdate;
        }
        public async Task AddUserToTravelAsync(int travelId, string passengerId)
        {

            var travel = await this.dbContext.Travels
           .Include(x => x.Car)
           .Include(x => x.StartLocation)
           .Include(x => x.EndLocation)
          // .Include(x => x.Passengers)
           .FirstOrDefaultAsync(x => x.Id == travelId);

            var passenger = await this.dbContext.Users
              .FirstOrDefaultAsync(x => x.Id == passengerId);

            travel.Passengers.Add(passenger);
            travel.AvailableSeats--;
          //  passenger.TravelHistory.Add(travel);

           await  this.dbContext.SaveChangesAsync();

        }

        public async Task RemoveUserToTravelAsync(int travelId, string passengerId)
        {
            var travel = await this.dbContext.Travels
             .Include(x => x.Car)
             .Include(x => x.StartLocation)
             .Include(x => x.EndLocation)
       //      .Include(x => x.Passengers)
             .FirstOrDefaultAsync(x => x.Id == travelId);

            var passenger = await this.dbContext.Users
              .FirstOrDefaultAsync(x => x.Id == passengerId);

            if (!(travel.Car.AvailableSeats == travel.AvailableSeats))
            {
                travel.Passengers.Remove(passenger);
                travel.AvailableSeats++;
                passenger.TravelHistory.Remove(travel);
            }

            await this.dbContext.SaveChangesAsync();
        }



        public async Task<IEnumerable<Travel>> FilterTravelsAndSortAsync(string sortBy)
        {
            IQueryable<Travel> travels = dbContext.Travels
                .Include(x => x.StartLocation)
                .Include(x => x.EndLocation)
                .Include(x => x.Car)
                .Where(c => c.IsDeleted == false);

            switch (sortBy.ToLower())
            {
                case "create":
                    travels = travels.OrderBy(t => t.CreatedOn);
                    break;
                case "startlocation":
                    travels = travels.OrderBy(t => t.StartLocation);
                    break;
                case "endlocation":
                    travels = travels.OrderBy(t => t.EndLocation);
                    break;
                case "departuretime":
                    travels = travels.OrderBy(t => t.DepartureTime);
                    break;
                case "arrivaltime":
                    travels = travels.OrderBy(t => t.ArrivalTime);
                    break;
                case "feedbacks":
                    travels = travels.OrderByDescending(t => t.Feedbacks.Count());
                    break;
                case "slots":
                    travels = travels.OrderByDescending(t => t.AvailableSeats);
                    break;
                default:
                    travels = travels.OrderBy(travel => travel.Id);
                    break;
            }
            if (travels.Count() > 0)
            {
                return await travels.ToListAsync();
            }

            throw new EmptyListException("No travels added yet!");

        }



    }
}
