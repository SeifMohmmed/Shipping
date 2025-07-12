using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;

public class SpecialCourierRegion
{
    public int Id { get; set; }

    //-----------  Region  ---------------------------------
    [ForeignKey(nameof(Region))]
    public int RegionId { get; set; }

    public virtual Region? Region { get; set; }

}