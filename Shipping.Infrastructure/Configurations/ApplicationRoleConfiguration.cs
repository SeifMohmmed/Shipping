using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.Infrastructure.Configurations;
public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        // Seed ApplicationRole entities
        builder.HasData(new ApplicationRole
        {
            Id = DefaultRole.AdminRoleId,
            Name = DefaultRole.Admin,
            NormalizedName = DefaultRole.Admin.ToUpper(),
            ConcurrencyStamp = DefaultRole.AdminRoleConcurrencyStamp
        });

        builder.HasData(new ApplicationRole
        {
            Id = DefaultRole.CourierRoleId,
            Name = DefaultRole.Courier,
            NormalizedName = DefaultRole.Courier.ToUpper(),
            ConcurrencyStamp = DefaultRole.CourierRoleConcurrencyStamp
        });

        builder.HasData(new ApplicationRole
        {
            Id = DefaultRole.MerchantRoleId,
            Name = DefaultRole.Merchant,
            NormalizedName = DefaultRole.Merchant.ToUpper(),
            ConcurrencyStamp = DefaultRole.MerchantRoleConcurrencyStamp
        });
    }
}