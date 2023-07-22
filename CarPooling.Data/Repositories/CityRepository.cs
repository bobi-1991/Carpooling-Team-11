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
    public class CityRepository : ICityRepository
    {
        private readonly CarPoolingDbContext _context;
        public CityRepository(CarPoolingDbContext context)
        {
            _context = context;
        }
        public City Create(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();
            return city;
        }

        public City Delete(int id)
        {
            City cityToDelete = GetById(id);
            cityToDelete.IsDelete = true;
            _context.SaveChanges();
            return cityToDelete;
        }

        public List<City> GetAll()
        {          
            return _context.Cities
                .Include(c=>c.Country)
                .Include(c=>c.Addresses)
                .Include(c=>c.Users)
                .ToList();
        }

        public City GetById(int id)
        {
            City city = _context.Cities.Where(c=>c.Id == id)
                .Include(c => c.Country)
                .Include(c => c.Addresses)
                .Include(c => c.Users)
                .FirstOrDefault();
            return city ?? throw new EntityNotFoundException($"No city found with this id: {id}!");
        }

        public City GetByName(string name)
        {
            City city = _context.Cities.Where(c => c.Name == name)
                .Include(c => c.Country)
                .Include(c => c.Addresses)
                .Include(c => c.Users)
                .FirstOrDefault();
            return city ?? throw new EntityNotFoundException($"No city found with this name: {name}!");
        }

        public City Update(int id, City city)
        {
            City cityToUpdate = GetById(id);
            cityToUpdate.Name = city.Name;
            _context.Update(cityToUpdate);
            _context.SaveChanges();
            return cityToUpdate;
        }
    }
}
