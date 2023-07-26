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

            builder.HasOne(address => address.Country)
                .WithMany(c => c.Addresses)
                .HasForeignKey(address => address.CountryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
