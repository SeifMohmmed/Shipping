using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.Product.DTOs;
public class BaseProductDTO
{
    public string Name { get; set; }
    public decimal Weight { get; set; }
    public int Quantity { get; set; }
}

public class ProductDTO : BaseProductDTO
{
    [JsonIgnore]
    public int Id { get; set; }
    public int OrderId { get; set; }
}
public class UpdateProductDTO : BaseProductDTO
{

}
