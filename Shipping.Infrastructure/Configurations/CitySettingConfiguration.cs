using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipping.Domain.Entities;

namespace Shipping.Infrastructure.Configurations;
public class CitySettingConfiguration : IEntityTypeConfiguration<CitySetting>
{
    public void Configure(EntityTypeBuilder<CitySetting> builder)
    {
        builder.Property(c => c.StandardShippingCost)
               .HasPrecision(18, 2);

        builder.Property(c => c.PickUpShippingCost)
               .HasPrecision(18, 2);
    }
}
