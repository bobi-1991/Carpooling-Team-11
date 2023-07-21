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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            builder.ApplyConfiguration(new UserConfiguration());
        }


    }
}
