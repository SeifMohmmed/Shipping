using Microsoft.AspNetCore.Identity;
using Shipping.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Id = Guid.CreateVersion7().ToString();
        SecurityStamp = Guid.CreateVersion7().ToString();
    }
    public string FullName { get; set; } = string.Empty;

    public bool IsDeleted { get; set; } = false;

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    //----------- StoreName For Merchant -------------------------------  
    public string? StoreName { get; set; }

    public decimal? PickupPrice { get; set; }

    public decimal? CanceledOrder { get; set; }

    public DeductionTypes? DeductionTypes { get; set; }

    public decimal? DeductionCompanyFromOrder { get; set; }

    [ForeignKey(nameof(Branch))]
    public int? BranchId { get; set; }

    public virtual Branch? Branch { get; set; }


    [ForeignKey(nameof(Region))]
    public int? RegionId { get; set; }
    public virtual Region? Region { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = [];

    public virtual ICollection<CourierReport> CourierReports { get; set; } = [];

    public virtual ICollection<SpecialCityCost> SpecialPickups { get; set; } = [];

    public virtual ICollection<SpecialCourierRegion> SpecialCourierRegion { get; set; } = [];

    public List<RefreshToken>? RefreshTokens { get; set; }

}
