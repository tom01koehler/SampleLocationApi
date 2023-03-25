using LocationLibrary.Contracts.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationLibrary.Data
{
    public class LocationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(p => p.Id).ValueGeneratedOnAdd();
                entity.HasIndex(i => i.Name).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
        public LocationDbContext() { }

        public LocationDbContext(DbContextOptions<LocationDbContext> options): base(options) { }

        public virtual DbSet<Location> Locations { get; set; }
    }
}
