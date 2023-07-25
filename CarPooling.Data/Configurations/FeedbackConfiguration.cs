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
            builder.Property(f=>f.Comment).IsRequired();
            builder.Property(f => f.Comment).HasMaxLength(200);

            //Relations
            builder.HasOne(f => f.Author)
                .WithMany(f => f.AuthorFeedbacks)
                .HasForeignKey(f => f.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Recipient)
                .WithMany(f => f.RecipientFeedbacks)
                .HasForeignKey(f => f.RecipientId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
