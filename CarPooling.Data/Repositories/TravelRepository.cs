using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            //           public User? Driver { get; set; }
            //public string? DriverId { get; set; }

            ////public int? StartLocationId { get; set; }
            //public Address? StartLocation { get; set; }

            ////public int? EndLocationId { get; set; }
            //public Address? EndLocation { get; set; }

            //public DateTime DepartureTime { get; set; }

            //public bool? IsCompleted { get; set; }

            ////public int? CarId { get; set; }
            //public Car? Car { get; set; }

            //public List<User>? Passengers { get; set; } = new List<User>();
            //public List<Feedback>? Feedbacks { get; set; } = new List<Feedback>();

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
               .Include(x => x.EndLocation)
               .Where(x => x.IsCompleted == false)
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



        public Task<Travel> UpdateAsync(int travelId, Travel travel)
        {
            throw new NotImplementedException();
        }
        public async Task<Travel> AddUserToTravelAsync(string driverId, int travelId, string passengerId)
        {
            throw new NotImplementedException();
            // var travel = await this.dbContext.Travels
            //.Include(x => x.Car)
            //.Include(x => x.Status)
            //.Include(x => x.Destination)
            //.Include(x => x.StartLocation)
            //.Include(x => x.Passenger)
            //.Where(x => x.Status.Id == 1 || x.StatusId == 2)
            //.FirstOrDefaultAsync(x => x.Id == travelId);


            // var user = await this.dbContext.Users
            //   .Include(x => x.Travel)
            //   .FirstOrDefaultAsync(x => x.Id == userId);


            // travel.Passenger.Add(user);
            // travel.AvailableSeat--;
            // user.Travel = travel;
            // user.TravelHistory.Add(travel);

            // var response = new TravelDTO(travel);

            // return response;
        }

        public async Task<IEnumerable<Travel>> FilterTravelsAndSortAsync( string sortBy)
        {
            IQueryable<Travel> travels = dbContext.Travels
                .Include(x=>x.StartLocation)
                .Include(x=>x.EndLocation)
                .Include(x=>x.Car)
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
