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
    public class CarRepository : ICarRepository
    {
        private readonly CarPoolingDbContext _context;
        public CarRepository(CarPoolingDbContext context)
        {
            _context = context;
        }
        public Car Create(Car car)
        {
            if(_context.Cars.Any(c=>c.Registration.Equals(car.Registration)))
            {
                throw new DuplicateEntityException("Such car already exists!");
            }
            car.CreatedOn = DateTime.Now;
            _context.Cars.Add(car);   
            _context.SaveChanges();
            return car;
        }

        public Car Delete(int id)
        {
            Car carToDelete = GetById(id);

            carToDelete.IsDeleted = true;
            carToDelete.DeletedOn = DateTime.Now;

            _context.SaveChanges();
            return carToDelete;
        }

        public List<Car> FilterCarsAndSort(string sortBy)
        {
            IQueryable<Car> cars = _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Model)
                .Include(c => c.Registration)
                .Include(c => c.Driver)
                .Include(c => c.CreatedOn)
                .Include(c => c.UpdatedOn)
                .Include(c => c.DeletedOn);
            

            switch(sortBy)
            {
                case "create":
                    cars=cars.OrderBy(c=>c.CreatedOn); 
                    break;
                case "brand":
                    cars = cars.OrderBy(cars => cars.Brand);
                    break;
                case "model":
                    cars=cars.OrderBy(cars => cars.Model);
                    break;
                default:
                    cars=cars.OrderBy(cars=>cars.Id);
                    break;
            }
            return cars.ToList();
        }

        public List<Car> GetAll()
        {
            return _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Model)
                .Include(c => c.Registration)
                .Include(c => c.Driver)
                .Include(c => c.CreatedOn)
                .Include(c => c.UpdatedOn)
                .Include(c => c.DeletedOn)
                .ToList();
        }

        public Car GetByBrand(string brandName)
        {
            Car car = _context.Cars
                .Where(c => c.Brand.Equals(brandName))
                .Include(c => c.Id)
                .Include(c => c.Model)
                .Include(c => c.Registration)
                .Include(c => c.Driver)
                .Include(c => c.CreatedOn)
                .Include(c => c.UpdatedOn)
                .Include(c => c.DeletedOn)
                .FirstOrDefault();

            return car ?? throw new EntityNotFoundException("Not existing car with such brand!");
        }

        public Car GetById(int id)
        {
            Car car = _context.Cars
                .Where(c => c.Id == id)
                .Include(c=>c.Brand)
                .Include(c=>c.Model)
                .Include(c=>c.Registration)
                .Include(c=>c.Driver) 
                .Include(c => c.CreatedOn)
                .Include(c => c.UpdatedOn)
                .Include(c => c.DeletedOn)
               .FirstOrDefault();

            return car ?? throw new EntityNotFoundException("There is no such car !");
        }

        public Car Update(int id, Car car)
        {
            Car carToUpdate = GetById(id);

            carToUpdate.TotalSeats=car.TotalSeats;
            carToUpdate.AvailableSeats=car.AvailableSeats;
            carToUpdate.Color=car.Color;
            carToUpdate.Registration = car.Registration;
            carToUpdate.CanSmoke= car.CanSmoke;
            carToUpdate.Brand=car.Brand;
            carToUpdate.Model = car.Model;
            carToUpdate.UpdatedOn = car.UpdatedOn;

            _context.Update(carToUpdate);
            _context.SaveChanges();

            return carToUpdate;
        }
    }
}
