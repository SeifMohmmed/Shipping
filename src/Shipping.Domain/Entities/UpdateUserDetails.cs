using Shipping.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shipping.Domain.Entities;
public class UpdateUserDetails
{
    public string FullName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;

    public string? Address { get; set; }

    public string? StoreName { get; set; }

    public decimal? PickupPrice { get; set; }

    public decimal? CanceledOrder { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public DeductionTypes? DeductionTypes { get; set; }

    public decimal? DeductionCompanyFromOrder { get; set; }

    public int? BranchId { get; set; }

    public int? RegionId { get; set; }
}
