using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shipping.Domain.Entities;

namespace Shipping.Infrastructure.Persistence;
internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    internal DbSet<Branch> Branches { get; set; }
    internal DbSet<CitySetting> CitySettings { get; set; }
    internal DbSet<CourierReport> CourierReports { get; set; }
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<Product> Products { get; set; }
    internal DbSet<Region> Regions { get; set; }
    internal DbSet<ShippingType> ShippingTypes { get; set; }
    internal DbSet<WeightSetting> WeightSettings { get; set; }
    internal DbSet<SpecialCityCost> SpecialCityCosts { get; set; }
    internal DbSet<SpecialCourierRegion> SpecialCourierRegions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CitySetting>()
                    .Property(c => c.StandardShippingCost)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<CitySetting>()
                    .Property(c => c.PickUpShippingCost)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
                    .Property(o => o.OrderCost)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
                    .Property(o => o.TotalWeight)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
                    .Property(p => p.Weight)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<ShippingType>()
                    .Property(s => s.BaseCost)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<WeightSetting>()
                    .Property(w => w.CostPerKg)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<WeightSetting>()
                    .Property(w => w.MaxWeight)
                    .HasPrecision(18, 2);

        modelBuilder.Entity<WeightSetting>()
                   .Property(w => w.MinWeight)
                   .HasPrecision(18, 2);

        modelBuilder.Entity<SpecialCityCost>()
                   .Property(s => s.Price)
                   .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
                   .Property(o => o.ShippingCost)
                   .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
                    .HasOne(o => o.Region)
                    .WithMany(r => r.Orders)
                    .HasForeignKey(o => o.RegionId)
                    .OnDelete(DeleteBehavior.Restrict); // avoids cycle
    }

}
