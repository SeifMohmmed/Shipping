using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.ShippingType.DTOs;

public class ShippingTypeBaseDTO
{
    public string Name { get; set; } = default!;

    public decimal BaseCost { get; set; }

    public int Duration { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<int> OrdersIds { get; set; } = [];
}

public class ShippingTypeDTO : ShippingTypeBaseDTO
{
    public int Id { get; set; }

}

public class ShippingTypeUpdateDTO : ShippingTypeBaseDTO
{

}

public class ShippingTypeAddDTO : ShippingTypeBaseDTO
{
    [JsonIgnore]
    public int Id { get; set; }
}