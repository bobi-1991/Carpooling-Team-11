using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

namespace CarPooling.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasKey(u => u.Id);
            builder.Property(u => u.FirstName).IsRequired(false);
            builder.Property(u => u.LastName).IsRequired(false);
            builder.Property(u => u.AddressId).IsRequired(false);

            //builder.HasOne(u => u.Travel)
            //    .WithMany(t => t.Passengers)
            //    .HasForeignKey(u => u.TravelId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(u => u.Address)
            //    .WithMany(a => a.Users)
            //    .HasForeignKey(u => u.AddressId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasOne(u => u.City)
            //    .WithMany(c => c.Users)
            //    .HasForeignKey(u => u.CityId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //// Collections
            //builder.HasMany(u => u.AuthorFeedbacks)
            //    .WithOne(f => f.Author)
            //    .HasForeignKey(f => f.AuthorId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasMany(u => u.RecipientFeedbacks)
            //    .WithOne(f => f.Recipient)
            //    .HasForeignKey(f => f.RecipientId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasMany(u => u.AuthorTripRequests)
            //    .WithOne(tr => tr.Author)
            //    .HasForeignKey(tr => tr.AuthorId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.HasMany(u => u.Cars)
            //    .WithOne(c => c.Driver)
            //    .HasForeignKey(c => c.DriverId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
