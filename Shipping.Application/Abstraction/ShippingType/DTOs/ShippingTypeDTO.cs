namespace Shipping.Application.Abstraction.ShippingType.DTOs;
public class ShippingTypeDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public decimal BaseCost { get; set; }

    public int Duration { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<int> OrdersIds { get; set; } = [];

}
