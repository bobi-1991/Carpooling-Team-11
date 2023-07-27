using CarPooling.Data.Data;
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

        public TravelRepository(CarPoolingDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        public Task<Travel> CreateTravelAsync(Travel travel)
        {
            throw new NotImplementedException();
        }

        public Task<Travel> DeleteAsync(int travelId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Travel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Travel> GetByIdAsync(int travelId)
        {
            throw new NotImplementedException();
        }

        public Task<Travel> UpdateAsync(int travelId, Travel travel)
        {
            throw new NotImplementedException();
        }
    }
}
