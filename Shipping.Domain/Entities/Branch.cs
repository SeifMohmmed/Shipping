using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;
public class Branch
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    public string Location { get; set; } = default!;

    public DateTime BranchDate { get; set; } = DateTime.Now;

    public bool IsDeleted { get; set; }


    [ForeignKey(nameof(Region))]
    public int? RegionId { get; set; }

    public virtual Region? Regions { get; set; }
}
