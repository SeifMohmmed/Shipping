using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipping.Domain.Entities;

namespace Shipping.Infrastructure.Configurations;
public class WeightSettingConfiguration : IEntityTypeConfiguration<WeightSetting>
{
    public void Configure(EntityTypeBuilder<WeightSetting> builder)
    {
        builder.Property(w => w.CostPerKg)
               .HasPrecision(18, 2);

        builder.Property(w => w.MaxWeight)
               .HasPrecision(18, 2);

        builder.Property(w => w.MinWeight)
               .HasPrecision(18, 2);
    }
}
