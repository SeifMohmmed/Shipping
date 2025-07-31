using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.Infrastructure.Persistence;
internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{
    internal DbSet<Branch> Branches { get; set; }
    internal DbSet<CitySetting> CitySettings { get; set; }
    internal DbSet<CourierReport> CourierReports { get; set; }
    internal DbSet<Order> Orders { get; set; }
    internal DbSet<OrderReport> OrderReports { get; set; }
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Define role-specific permission subsets
        string[] CourierPermissions = [
            Permissions.ViewOrders,
            Permissions.UpdateOrders,
        ];

        string[] MerchantPermissions = [
            Permissions.ViewOrders,
            Permissions.AddOrders,
        ];

        var permissions = Permissions.GetAllPermissions();
        var adminClaims = new List<IdentityRoleClaim<string>>();
        var courierClaims = new List<IdentityRoleClaim<string>>();
        var merchantClaims = new List<IdentityRoleClaim<string>>();

        int claimId = 1;

        // Admin gets all permissions
        foreach (var permission in permissions)
        {
            adminClaims.Add(new IdentityRoleClaim<string>
            {
                Id = claimId++,
                RoleId = DefaultRole.AdminRoleId,
                ClaimType = Permissions.Type,
                ClaimValue = permission
            });
        }

        // Courier gets their subset
        foreach (var permission in CourierPermissions)
        {
            courierClaims.Add(new IdentityRoleClaim<string>
            {
                Id = claimId++,
                RoleId = DefaultRole.CourierRoleId,
                ClaimType = Permissions.Type,
                ClaimValue = permission
            });
        }

        // Merchant gets their subset
        foreach (var permission in MerchantPermissions)
        {
            merchantClaims.Add(new IdentityRoleClaim<string>
            {
                Id = claimId++,
                RoleId = DefaultRole.MerchantRoleId,
                ClaimType = Permissions.Type,
                ClaimValue = permission
            });
        }

        builder.Entity<IdentityRoleClaim<string>>()
               .HasData(adminClaims);
        builder.Entity<IdentityRoleClaim<string>>()
           .HasData(courierClaims);
        builder.Entity<IdentityRoleClaim<string>>()
           .HasData(merchantClaims);
    }

}
