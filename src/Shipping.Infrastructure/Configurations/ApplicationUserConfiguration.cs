using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.Infrastructure.Configurations;
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var hashedPassword = passwordHasher.HashPassword(null!, DefaultUser.AdminPassword);

        builder.Property(a => a.CanceledOrder)
               .HasPrecision(18, 2);

        builder.Property(a => a.DeductionCompanyFromOrder)
               .HasPrecision(18, 2);

        builder.Property(a => a.PickupPrice)
               .HasPrecision(18, 2);


        builder.HasData(new ApplicationUser
        {
            Id = DefaultUser.AdminId,
            FullName = "Seif Admin",
            UserName = DefaultUser.AdminEmail,
            NormalizedUserName = DefaultUser.AdminEmail.ToUpper(),
            Email = DefaultUser.AdminEmail,
            NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUser.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUser.AdminConcurrencyStamp,
            PasswordHash = hashedPassword
        });
    }
}
