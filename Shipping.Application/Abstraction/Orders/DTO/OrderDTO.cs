using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.Orders.DTO;
public class OrderBaseDTO
{
    public decimal OrderCost { get; set; }

    public string MerchantName { get; set; } = string.Empty;

    public string CustomerName { get; set; } = default!;

    public string CustomerPhone { get; set; } = default!;

    public string CustomerAddress { get; set; } = default!;

    public string CustomerEmail { get; set; } = default!;

    public List<ProductDTO> Products { get; set; } = new();
}

public class OrderWithProductsDTO : OrderBaseDTO
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public string? Notes { get; set; }

    public required string Status { get; set; }

    public string? Branch { get; set; }

    public string? Region { get; set; }

    public string? City { get; set; }

    public decimal OrderCost { get; set; }

    public string CourierId { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;

}

public class AddOrderDTO : OrderBaseDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    public OrderType OrderTypes { get; set; }

    public bool IsOutOfCityShipping { get; set; }

    public int ShippingId { get; set; }

    public PaymentType? PaymentType { get; set; }

    public new int? Branch { get; set; } // new int ==> hide from Update DTO

    public new int? Region { get; set; }// new int ==> hide from Update DTO

    public new int City { get; set; }// new int ==> hide from Update DTO

    public string CourierId { get; set; } = string.Empty;  // add this

    public decimal TotalWeight { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Order cost must be greater than zero")]
    public decimal OrderCost { get; set; }

    [JsonIgnore]
    public decimal ShippingCost { get; set; }

    [MinLength(1, ErrorMessage = "At least one product is required")]
    public List<ProductDTO> Products { get; set; } = new();

    [JsonIgnore]
    public OrderStatus status { get; set; }

}
public class UpdateOrderDTO : AddOrderDTO
{

}