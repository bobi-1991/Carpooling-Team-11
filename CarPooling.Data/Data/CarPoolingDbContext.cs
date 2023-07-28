using CarPooling.Data.Configurations;
using CarPooling.Data.DatabaseSeeder;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Data.Data
{
    public class CarPoolingDbContext : IdentityDbContext<User, UserRole, string>
    {
        public CarPoolingDbContext() { }

        public CarPoolingDbContext(DbContextOptions<CarPoolingDbContext> options)
          : base(options) { }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Travel> Travels { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<TripRequest> TripRequests  { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new FeedbackConfiguration());
            builder.ApplyConfiguration(new AddressConfiguration());
            builder.ApplyConfiguration(new CarConfiguration());
            builder.ApplyConfiguration(new TravelConfiguration());
            builder.ApplyConfiguration(new CountryConfiguration());
            builder.ApplyConfiguration(new TripRequestConfiguration());
            
            //builder.Seeder();
            base.OnModelCreating(builder);
        }


    }
}
