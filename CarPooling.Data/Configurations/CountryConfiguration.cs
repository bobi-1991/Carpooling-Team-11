using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            //Primary Key
            builder.HasKey(key => key.Id);

            //Required Fields
            builder.Property(address => address.Name).IsRequired();
            builder.Property(address => address.Name).HasMaxLength(50);


            //Relations
            builder.HasMany(country => country.Cities)
                .WithOne(city => city.Country)
                .HasForeignKey(city => city.CountryId)
                .OnDelete(DeleteBehavior.NoAction); 

        }
    }
}
