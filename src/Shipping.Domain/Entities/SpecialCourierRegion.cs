using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;

public class SpecialCourierRegion
{
    public int Id { get; set; }

    [ForeignKey(nameof(Region))]
    public int RegionId { get; set; }

    public virtual Region? Region { get; set; }


    [ForeignKey(nameof(Courier))]
    public string CourierId { get; set; } = string.Empty;

    public virtual ApplicationUser? Courier { get; set; }

}