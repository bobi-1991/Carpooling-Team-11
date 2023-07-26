using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPooling.Data.Configurations
{
    public class TripRequestConfiguration : IEntityTypeConfiguration<TripRequest>
    {
        public void Configure(EntityTypeBuilder<TripRequest> builder)
        {
            //Primary Key
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Driver)
                      .WithMany(t => t.AuthorTripRequests)
                      .HasForeignKey(t => t.DriverId)
                      .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
