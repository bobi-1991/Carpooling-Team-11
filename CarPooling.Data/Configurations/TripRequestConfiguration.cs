using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarPooling.Data.Configurations
{
    public class TripRequestConfiguration : IEntityTypeConfiguration<TripRequest>
    {
        public void Configure(EntityTypeBuilder<TripRequest> builder)
        {
            //Primary Key
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.Passenger)
                      .WithMany(t => t.PassengerTripRequests)
                      .HasForeignKey(t => t.PassengerId)
                      .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Travel);
        }
    }
}
