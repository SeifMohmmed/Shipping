using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.SpecialCourierRegion.DTO;
public class SpecialCourierRegionDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    public int RegionId { get; set; }

    public string? RegionName { get; set; }

    public string? CourierId { get; set; }

    public string? CourierName { get; set; }

}
