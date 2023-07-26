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
    public class TravelConfiguration : IEntityTypeConfiguration<Travel>
    {

        public void Configure(EntityTypeBuilder<Travel> builder)
        {
            // Primary key
            builder.HasKey(t => t.Id);

            //Required Fields
            //builder.Property(t => t.StartLocation).IsRequired();
            //builder.Property(t => t.Destination).IsRequired();
            //builder.Property(t => t.Car).IsRequired();

            //Relations
            //builder.HasOne(t => t.StartLocation)
            //    .WithMany(sl => sl.Travels)
            //    .HasForeignKey(t => t.StartLocationId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(t => t.EndLocation);

            builder.HasOne(t => t.Driver)
                .WithMany(t => t.TravelHistory)
                .HasForeignKey(t => t.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(t => t.Car);


        }
    }
}
