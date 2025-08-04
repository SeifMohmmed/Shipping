using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shipping.Domain.Entities;

namespace Shipping.Infrastructure.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(o => o.OrderCost)
               .HasPrecision(18, 2);

        builder.Property(o => o.TotalWeight)
               .HasPrecision(18, 2);

        builder.Property(o => o.ShippingCost)
               .HasPrecision(18, 2);

        builder.HasOne(o => o.Region)
               .WithMany(r => r.Orders)
               .HasForeignKey(o => o.RegionId)
               .OnDelete(DeleteBehavior.Restrict); // avoids cycle

    }
}
