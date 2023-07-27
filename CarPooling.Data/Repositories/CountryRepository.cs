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
    public class CountryRepository : ICountryRepository
    {
        private readonly CarPoolingDbContext _context;
        public CountryRepository(CarPoolingDbContext context)
        {
            _context = context;
        }

        public Country Create(Country country)
        {
            if(_context.Countries.Any(c=>c.Name.Equals(country.Name)))
            {
                throw new DuplicateEntityException($"Country with this name: {country.Name} already exists!");
            }
            country.CreatedOn = DateTime.Now;
            _context.Countries.Add(country);
            _context.SaveChanges();
            return country;
        }

        public Country Delete(int id)
        {
            Country countryToDelete = GetById(id);
            countryToDelete.IsDeleted = true;
            countryToDelete.DeletedOn = DateTime.Now;
            _context.SaveChanges();
            return countryToDelete;
        }

        public List<Country> FilterCountriesByName(string orderByName)
        {
            IQueryable<Country> countries = _context.Countries;
            countries.OrderBy(c => c.Name);
            return countries.ToList();
        }

        public List<Country> GetAll()
        {
            return _context.Countries
                .Include(c => c.Name)
                .Include(c => c.CreatedOn)
                .Include(c => c.DeletedOn)
                .Include(c => c.UpdatedOn)
                .ToList();
        }

        public Country GetById(int id)
        {
            Country country = _context.Countries
                .Where(c=>c.Id == id)
                .Include(c=>c.Name)
                .Include(c=>c.CreatedOn)
                .Include (c=>c.DeletedOn)
                .Include(c=>c.UpdatedOn)
                .FirstOrDefault();
            return country ?? throw new EntityNotFoundException("Country not found!");
        }

        public Country Update(int id, Country country)
        {
            Country countryToUpdate = GetById(id);
            countryToUpdate.UpdatedOn = DateTime.Now ;
            countryToUpdate.Name = country.Name;
            _context.Update(countryToUpdate);
            _context.SaveChanges();
            return countryToUpdate;
        }
    }
}
