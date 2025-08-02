namespace Shipping.Domain.Entities;
public class ShippingType
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public decimal BaseCost { get; set; }

    public int Duration { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<Order> Orders { get; set; } = [];

}
