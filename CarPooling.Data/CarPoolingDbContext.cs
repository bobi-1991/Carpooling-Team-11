using CarPooling.Data.Configurations;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarPooling.Data
{
    public class CarPoolingDbContext : IdentityDbContext<User>
    {
        public CarPoolingDbContext() { }

        public CarPoolingDbContext(DbContextOptions<CarPoolingDbContext> options)
          : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new CityConfiguration());
            builder.ApplyConfiguration(new FeedbackConfiguration());
            builder.ApplyConfiguration(new AddressConfiguration());
            base.OnModelCreating(builder); 
        }


    }
}
