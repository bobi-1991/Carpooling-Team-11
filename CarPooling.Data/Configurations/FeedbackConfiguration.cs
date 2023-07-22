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

            //Required Fields
            builder.Property(f=>f.Body).IsRequired();
            builder.Property(f => f.Body).HasMaxLength(200);

            //Relations
            builder.HasOne(f => f.User)
                .WithMany(f => f.AuthorFeedbacks)
                .HasForeignKey(f => f.UserId);

            //builder.HasOne(f => f.Travel)
            //    .WithMany(f=>f.Feedbacks)
            //    .HasForeignKey(f=>f.TravelId);
        }
    }
}
