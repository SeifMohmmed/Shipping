using Shipping.Domain.Enums;
using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.OrderReport.DTO;
public abstract class BaseOrderReportDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    public DateTime ReportDate { get; set; } = DateTime.Now;
}

public class OrderReportDTO : BaseOrderReportDTO
{
    public string ReportDetails { get; set; } = string.Empty;

    public int? OrderId { get; set; }
}

public class OrderReportToShowDTO : BaseOrderReportDTO
{
    public bool IsDeleted { get; set; }

    public string? MerchantName { get; set; }

    [JsonIgnore]
    public string? MerchantId { get; set; }

    [JsonIgnore]
    public string? CourierId { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerPhone1 { get; set; }

    public string? RegionName { get; set; }

    public string? CityName { get; set; }

    public decimal OrderCost { get; set; }

    public decimal ShippingCost { get; set; }

    public decimal AmountReceived { get; set; }

    public decimal ShippingCostPaid { get; set; }

    public decimal? CompanyValue { get; set; }

    [JsonIgnore]
    public string PaymentType { get; set; } = string.Empty;

    public OrderStatus OrderStatus { get; set; }

}