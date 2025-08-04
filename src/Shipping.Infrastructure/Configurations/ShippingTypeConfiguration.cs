using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipping.Domain.Entities;

namespace Shipping.Infrastructure.Configurations;
public class ShippingTypeConfiguration : IEntityTypeConfiguration<ShippingType>
{
    public void Configure(EntityTypeBuilder<ShippingType> builder)
    {
        builder.Property(s => s.BaseCost)
               .HasPrecision(18, 2);
    }
}
