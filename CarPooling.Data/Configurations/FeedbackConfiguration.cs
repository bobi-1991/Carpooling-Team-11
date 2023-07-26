using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarPooling.Data.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            //Primary Key
            builder.HasKey(key => key.Id);

            ////Required Fields
            //builder.Property(f=>f.Comment).IsRequired();
            //builder.Property(f => f.Comment).HasMaxLength(200);

            //Relations
            builder.HasOne(f => f.Passenger)
                .WithMany(passenger => passenger.PassengerFeedbacks)
                .HasForeignKey(f => f.PassengerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Driver)
                .WithMany(driver => driver.DriverFeedbacks)
                .HasForeignKey(f => f.DriverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Travel)
               .WithMany(travel => travel.Feedbacks)
               .HasForeignKey(f => f.TravelId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
