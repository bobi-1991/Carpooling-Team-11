using CarPooling.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace CarPooling.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            //Primary Key
            builder.HasKey(key => key.Id);

            //Required Fields
            builder.Property(address=>address.Name).IsRequired();
            builder.Property(address => address.Name).HasMaxLength(50);
            builder.Property(address=>address.AddressNumber).IsRequired();
            builder.Property(address=>address.AddressNumber).HasPrecision(1, 300);

            //Relations
            builder.HasOne(address => address.City)
                .WithMany(City => City.Addresses)
                .HasForeignKey(address => address.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(address => address.Country)
                .WithMany(address => address.Addresses)
                .HasForeignKey(addres => addres.CountryId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasMany(a => a.Users)
                .WithOne(a => a.Address)
                .HasForeignKey(a => a.AddressId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
