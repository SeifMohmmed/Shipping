using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.Region.DTO;

public class RegionBaseDTO
{
    public required string Governorate { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<string> CityName { get; set; } = new();
}

public class RegionDTO : RegionBaseDTO
{
    public int Id { get; set; }
}

public class RegionToAddDTO : RegionBaseDTO
{
    [JsonIgnore]
    public int Id { get; set; }
}
