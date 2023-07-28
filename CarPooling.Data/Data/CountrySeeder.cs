using CarPooling.Data.JsonManager;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Data
{

    public static class CountrySeeder
    {
        private const string countriesDirectory = @"..\CarPooling.Data\JsonRaw\Countries2.json";
        private const string addressesDirectory = @"..\CarPooling.Data\JsonRaw\Addresses.json";
        public static void SeedDatabaseCountries(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<CarPoolingDbContext>();
                var _jsonManager = serviceScope.ServiceProvider.GetService<IJsonManager>();
                
                var countCountries = _context.Countries.Count();
                var countAddress = _context.Addresses.Count();
                
                if (countCountries == 0)
                {
                    _context.Database.Migrate();

                    var countries = _jsonManager.ExtractTypesFromJson<Country>(countriesDirectory);
                    _context.Countries.AddRange(countries);
                    _context.SaveChanges();
                }
                if (countAddress == 0)
                {
                    _context.Database.Migrate();

                    var addresses = _jsonManager.ExtractTypesFromJson<Address>(addressesDirectory);
                    _context.Addresses.AddRange(addresses);
                    _context.SaveChanges();
                }
            }
        }
    }
}

