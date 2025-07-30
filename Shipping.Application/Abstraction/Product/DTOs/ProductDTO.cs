namespace Shipping.Application.Abstraction.Product.DTOs;
public class ProductDTO
{
    public string Name { get; set; }
    public decimal Weight { get; set; }
    public int Quantity { get; set; }

}
public class UpdateProductDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Weight { get; set; }
    public int Quantity { get; set; }
}
