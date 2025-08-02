using Shipping.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;
public class Order
{
    public int Id { get; set; }

    [Required]
    public decimal TotalWeight { get; set; }

    [Required]
    public decimal OrderCost { get; set; }

    public decimal ShippingCost { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public bool IsOutOfCityShipping { get; set; } = false;

    public bool IsDeleted { get; set; } = false;

    public OrderStatus Status { get; set; }

    public OrderType OrderTypes { get; set; }

    //----------- Ids From User ---------------------------------
    public string MerchantId { get; set; } = string.Empty;

    public string EmployeeId { get; set; } = string.Empty;

    public string CourierId { get; set; } = default!;

    public virtual ICollection<CourierReport> CouriersReport { get; set; } = [];

    //------------- Branch ---------------------------------------

    [ForeignKey(nameof(Branch))]
    public int BranchId { get; set; }

    public virtual Branch? Branch { get; set; }

    //------------- Region ---------------------------------------

    [ForeignKey(nameof(Region))]
    public int RegionId { get; set; }

    public virtual Region? Region { get; set; }

    //------------- CitySetting ------------------------------

    [ForeignKey(nameof(CitySetting))]
    public int CitySettingId { get; set; }

    public virtual CitySetting? CitySetting { get; set; }

    //------------- ShippingType ------------------------------

    [ForeignKey(nameof(ShippingType))]
    public int ShippingTypeId { get; set; }

    public virtual ShippingType? ShippingType { get; set; }

    //----------------------------------------------------------------------
    public PaymentType? PaymentType { get; set; }

    public virtual ICollection<Product>? Products { get; set; }

    //----------- Customer Info ---------------------------------

    public string CustomerName { get; set; } = default!;

    public string CustomerPhone { get; set; } = default!;

    public string CustomerAddress { get; set; } = default!;

    public string CustomerEmail { get; set; } = default!;


}
