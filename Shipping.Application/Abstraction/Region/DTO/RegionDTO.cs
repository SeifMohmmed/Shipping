using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.Region.DTO;

public class RegionBaseDTO
{
    public required string Governorate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> CityName { get; set; } = new List<string>();
}

public class RegionDTO : RegionBaseDTO
{
    [JsonIgnore]
    public int Id { get; set; }

}
public class RegionUpdate : RegionBaseDTO
{

}
