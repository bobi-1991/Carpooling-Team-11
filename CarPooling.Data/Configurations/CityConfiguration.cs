using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarPooling.Data.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            //Primary Key
            builder.HasKey(key => key.Id);
            
            //Required Fields
            builder.Property(city=>city.Name).IsRequired();
            builder.Property(city => city.Name).HasMaxLength(20);

            //Relations
            //builder.HasOne(c => c.Country)
            //    .WithMany(c => c.Cities)
            //    .HasForeignKey(c => c.CountryId);

            builder.HasMany(c => c.Addresses)
                .WithOne(c => c.City)
                .HasForeignKey(c => c.CityId);

            builder.HasMany(c => c.Users)
                .WithOne(c => c.City)
                .HasForeignKey(c => c.CityId);
        }
    }
}
