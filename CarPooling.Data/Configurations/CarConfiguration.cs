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

            //builder.Property(c => c.Registration).IsRequired();
            //builder.Property(c => c.TotalSeats).IsRequired();
            //builder.Property(c => c.Brand).IsRequired();
            //builder.Property(c => c.Model).IsRequired();
            //builder.Property(c => c.Color).IsRequired();

            //Relations
            builder.HasOne(car => car.Driver)
                  .WithMany(user => user.Cars)
                  .HasForeignKey(car => car.DriverId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
