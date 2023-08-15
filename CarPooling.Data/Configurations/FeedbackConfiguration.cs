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
            builder.HasOne(f => f.Giver)
                .WithMany(passenger => passenger.ReceivedFeedbacks)
                .HasForeignKey(f => f.GiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Receiver)
                .WithMany(driver => driver.GivenFeedbacks)
                .HasForeignKey(f => f.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Travel)
               .WithMany(travel => travel.Feedbacks)
               .HasForeignKey(f => f.TravelId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
