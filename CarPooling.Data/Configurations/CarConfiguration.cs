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
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {

            //Primary Key
            builder.HasKey(c => c.Id);

            //Required Fields
            builder.Property(c => c.Registration).IsRequired();
            builder.Property(c => c.TotalSeats).IsRequired();
            builder.Property(c => c.Brand).IsRequired();
            builder.Property(c => c.Model).IsRequired();
            builder.Property(c => c.Color).IsRequired();

            //Relations
            builder.HasOne(car => car.User)
                  .WithMany(user => user.Cars)
                  .HasForeignKey(car => car.UserId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(car => car.Travels)
                   .WithOne(travel => travel.Car)
                   .HasForeignKey(travel => travel.CarId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
