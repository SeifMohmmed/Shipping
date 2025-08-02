using System.ComponentModel.DataAnnotations;

namespace Shipping.Domain.Entities;
public class Region
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Governorate { get; set; } = default!;

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<ApplicationUser> Users { get; set; } = [];

    public virtual ICollection<CitySetting> CitySettings { get; set; } = [];

    public virtual ICollection<Branch> Branches { get; set; } = [];

    public virtual ICollection<Order> Orders { get; set; } = [];

    public virtual ICollection<SpecialCourierRegion> SpecialCourierRegion { get; set; } = [];

}
