using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Country> CreateAsync(Country country)
        {
            if (await _context.Countries.AnyAsync(c => c.Name.Equals(country.Name)))
            {
                throw new DuplicateEntityException($"Country with this name: {country.Name} already exists!");
            }
            country.CreatedOn = DateTime.Now;
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<Country> DeleteAsync(int id)
        {
            Country countryToDelete = await GetByIdAsync(id);
            countryToDelete.IsDeleted = true;
            countryToDelete.DeletedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return countryToDelete;
        }

        public async Task<List<Country>> FilterCountriesByNameAsync(string orderByName)
        {
            IQueryable<Country> countries = _context.Countries.OrderBy(c => c.Name);
            return await countries.ToListAsync();
        }

        public async Task<List<Country>> GetAllAsync()
        {
            return await _context.Countries
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<Country> GetByIdAsync(int id)
        {
            Country country = await _context.Countries
                .Where(c => c.IsDeleted == false)
                .FirstOrDefaultAsync(c => c.Id == id);
            return country ?? throw new EntityNotFoundException("Country not found!");
        }

        public async Task<Country> UpdateAsync(int id, Country country)
        {
            Country countryToUpdate = await GetByIdAsync(id);
            countryToUpdate.UpdatedOn = DateTime.Now;
            countryToUpdate.Name = country.Name;
            _context.Update(countryToUpdate);
            await _context.SaveChangesAsync();
            return countryToUpdate;
        }
    }
}