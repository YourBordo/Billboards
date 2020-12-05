using Billboards.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBaseAccess.DbContext
{
    public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<DeviceGroup> DeviceGroups { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<AdvertisementStatistics> AdvertisementStatistics { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
           
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            base.OnConfiguring(optionsBuilder);
        }
    }
}