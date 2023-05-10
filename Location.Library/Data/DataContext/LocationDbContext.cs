using LocationLibrary.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace LocationLibrary.Data.DataContext
{
    [ExcludeFromCodeCoverage(Justification = "DB Context")]
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
