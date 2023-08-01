using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Data.Repositories
{
    public class CarRepository : ICarRepository 
    {
        private readonly CarPoolingDbContext _context;

        public CarRepository(CarPoolingDbContext context)
        {
            _context = context;
        }

        public async Task<Car> CreateAsync(Car car)
        {
            if (await _context.Cars.AnyAsync(c => c.Registration.Equals(car.Registration) && c.Model.Equals(car.Model) && c.Brand.Equals(car.Brand)))
            {
                throw new DuplicateEntityException("Such car already exists!");
            }
            car.CreatedOn = DateTime.Now;
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<Car> DeleteAsync(int id)
        {
            Car carToDelete = await GetByIdAsync(id);

            carToDelete.IsDeleted = true;
            carToDelete.DeletedOn = DateTime.Now;

            await _context.SaveChangesAsync();
            return carToDelete;
        }

        public async Task<List<Car>> FilterCarsAndSortAsync(string sortBy)
        {
            IQueryable<Car> cars = _context.Cars
                .Where(c => c.IsDeleted == false);

            switch (sortBy)
            {
                case "date":
                    cars = cars.OrderBy(c => c.CreatedOn).ThenBy(cars=>cars.Brand);
                    break;
                case "brand":
                    cars = cars.OrderBy(cars => cars.Brand).ThenBy(cars=>cars.Model).ThenBy(cars=>cars.CreatedOn);
                    break;
                case "model":
                    cars = cars.OrderBy(cars => cars.Model).ThenBy(cars=>cars.Brand).ThenBy(cars => cars.CreatedOn);
                    break;
                default:
                    cars = cars.OrderBy(cars => cars.Id);
                    break;
            }
            if(cars.Count() > 0) 
            {
                return await cars.ToListAsync();
            }
            throw new EmptyListException("No cars added yet!");          
        }

        public async Task<List<Car>> GetAllAsync()
        {
            if(_context.Cars.Count() == 0)
            {
                new EmptyListException("No cars added yet!");
            }
            return await _context.Cars
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Car> GetByBrandModelAndRegistrationAsync(string brandName, string model, string registration)
        {
            Car car = await _context.Cars
                .Where(c => c.IsDeleted == false && c.Brand.Equals(brandName) && c.Model.Equals(model) && c.Registration.Equals(registration))
                .FirstOrDefaultAsync();

            return car ?? throw new EntityNotFoundException("Not existing car with such brand and model!");
        }

        public async Task<Car> GetByIdAsync(int id)
        {
            Car car = await _context.Cars.Where(c => c.Id == id && c.IsDeleted == false)
                .FirstOrDefaultAsync();

            return car ?? throw new EntityNotFoundException("There is no such car!");
        }

        public async Task<Car> UpdateAsync(int id, Car car)
        {
            Car carToUpdate = await GetByIdAsync(id);

            carToUpdate.TotalSeats = car.TotalSeats;
            carToUpdate.AvailableSeats = car.AvailableSeats;
            carToUpdate.Color = car.Color;
            carToUpdate.Registration = car.Registration;
            carToUpdate.CanSmoke = car.CanSmoke;
            carToUpdate.Brand = car.Brand;
            carToUpdate.Model = car.Model;
            carToUpdate.UpdatedOn = DateTime.Now;

            _context.Update(carToUpdate);
            await _context.SaveChangesAsync();

            return carToUpdate;
        }
    }
}