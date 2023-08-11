using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Pagination;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Data.Repositories
{
    public class TravelRepository : ITravelRepository
    {
        private readonly CarPoolingDbContext dbContext;
        private readonly IUserRepository userRepository;
        private readonly ITripRequestRepository tripRequestRepository;

        public TravelRepository(CarPoolingDbContext dbContext, IUserRepository userRepository, ITripRequestRepository tripRequestRepository)
        {
            this.dbContext = dbContext;
            this.userRepository = userRepository;
            this.tripRequestRepository = tripRequestRepository;
        }

        public async Task<IEnumerable<Travel>> GetAllAsync()
        {
            var result = await this.dbContext.Travels
                    .Include(x => x.Car)
                    .Include(x => x.Driver)
                    .Include(x => x.StartLocation)
                    .Include(x => x.EndLocation)
                    .Where(x => x.IsCompleted == false && !x.IsDeleted)
                    .ToListAsync();

            if(result.Count()  == 0) {
                throw new EmptyListException("No travels yet!");
            }
            return result;
        }

        public async Task<Travel> GetByIdAsync(int travelId)
        {
            var travel = await this.dbContext.Travels
               .Include(x => x.Car)
               .Include(x => x.StartLocation)
                  .ThenInclude(x => x.Country)
               .Include(x => x.EndLocation)
                    .ThenInclude(x => x.Country)
             //  .Where(x => x.IsCompleted == false)
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
            var driver = await this.userRepository.GetByIdAsync(travelToDelete.DriverId);
            

            driver.TravelHistory.Remove(travelToDelete);

            this.dbContext.Update(driver);

            travelToDelete.IsDeleted = true;
            travelToDelete.DeletedOn = DateTime.Now;

            //NEW
            var tripRequests = await this.tripRequestRepository.GetAllAsync();
            var tripRequestsForDelete = tripRequests.Where(x => x.Travel.IsDeleted);



            foreach (var trip in tripRequestsForDelete)
            {
                trip.IsDeleted = true;
                dbContext.TripRequests.Update(trip);
            }

            await dbContext.SaveChangesAsync();

            return "Travel successfully deleted.";
        }

        public async Task<string> SetTravelToIsCompleteAsync(Travel travel)
        {
            travel.IsCompleted = true;

            this.dbContext.Update(travel);
            await this.dbContext.SaveChangesAsync();

            await dbContext.SaveChangesAsync();

            return "Travel successfully set to completed.";
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
            //NEW
            this.dbContext.Travels.Update(travel);
            travel.AvailableSeats--;
            //NEW
            //May be wrong
            //passenger.TravelHistory.Add(travel);
            this.dbContext.Users.Update(passenger);


            await this.dbContext.SaveChangesAsync();

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

        // FilterBy logic:

        public async Task<IQueryable<Travel>> GetAllToQueriable()
        {
            IQueryable<Travel> result = this.dbContext.Travels
                    .Include(x => x.Car)
                    .Include(x => x.Driver)
                    .Include(x => x.StartLocation)
                    .Include(x => x.EndLocation)
                    .Where(x => x.IsCompleted == false && !x.IsDeleted);

            return result;
        }
        public async Task<PaginatedList<Travel>> FilterByAsync(TravelQueryParameters filter)
        {


            IQueryable<Travel> travels = await this.GetAllToQueriable();

            travels = FilterByDriverUsername(travels, filter.DriverUsername);
            travels = FilterByStartLocation(travels, filter.StartLocation);
            travels = FilterByEndLocation(travels, filter.EndLocation);
            if (filter.AvailableSeats.HasValue)
            {
                travels = FilterByAvailableSeats(travels, (int)filter.AvailableSeats.Value);
            }
            //            travels = FilterByAvailableSeats(travels, (int)filter.AvailableSeats);
            travels = SortBy(travels, filter.SortBy);



            int totalPages = (travels.Count() + 1) / filter.PageSize;
            var result = Paginate(travels, filter.PageNumber, filter.PageSize);

            return new PaginatedList<Travel>(result, totalPages, filter.PageNumber);
        }
        public static List<Travel> Paginate(IQueryable<Travel> result, int pageNumber, int pageSize)
        {
            return result
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToList();
        }
        private static IQueryable<Travel> FilterByDriverUsername(IQueryable<Travel> travels, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                return travels.Where(travel => travel.Driver.UserName == username);
            }

            return travels;
        }

        private static IQueryable<Travel> FilterByStartLocation(IQueryable<Travel> travels, string startLocation)
        {
            if (!string.IsNullOrEmpty(startLocation))
            {
                return travels.Where(travel => travel.StartLocation.City == startLocation);
            }

            return travels;
        }
        private static IQueryable<Travel> FilterByEndLocation(IQueryable<Travel> travels, string endLocation)
        {
            if (!string.IsNullOrEmpty(endLocation))
            {
                return travels.Where(travel => travel.EndLocation.City == endLocation);
            }

            return travels;
        }
        private static IQueryable<Travel> FilterByAvailableSeats(IQueryable<Travel> travels, int availableSeats)
        {
            if (availableSeats >= 0 && availableSeats <= 4)
            {
                return travels.Where(travel => travel.AvailableSeats == availableSeats);
            }

            return travels;
        }
        private static IQueryable<Travel> SortBy(IQueryable<Travel> travels, string sortCriteria)
        {
            switch (sortCriteria)
            {
                case "username":
                    return travels.OrderBy(travel => travel.Driver.UserName);
                case "start location":
                    return travels.OrderBy(travel => travel.StartLocation.City);
                case "end location":
                    return travels.OrderBy(travel => travel.EndLocation.City);
                case "available seats":
                    return travels.OrderBy(travel => travel.AvailableSeats);
                default:
                    return travels;
            }
        }

   

    }
}
