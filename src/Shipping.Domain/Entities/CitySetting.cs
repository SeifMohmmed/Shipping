using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;
public class CitySetting
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public decimal StandardShippingCost { get; set; }

    public decimal PickUpShippingCost { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [ForeignKey(nameof(Region))]
    public int RegionId { get; set; }

    public virtual Region Region { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = [];

    public virtual ICollection<SpecialCityCost> SpecialPickups { get; set; } = [];

    public virtual ICollection<ApplicationUser> Users { get; set; } = [];

}
