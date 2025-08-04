using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipping.Domain.Entities;

namespace Shipping.Infrastructure.Configurations;
public class SpecialCityCostConfiguration : IEntityTypeConfiguration<SpecialCityCost>
{
    public void Configure(EntityTypeBuilder<SpecialCityCost> builder)
    {
        builder.Property(s => s.Price)
               .HasPrecision(18, 2);
    }
}
