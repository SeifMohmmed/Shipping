using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.CitySettings.DTO;
public class CitySettingDTO
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal StandardShippingCost { get; set; }

    public decimal PickUpShippingCost { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int RegionId { get; set; }

    public string? RegionName { get; set; }

    //------------- List From User ------------------------------
    public List<string> UsersName { get; set; } = [];

    //------------- List From Order ------------------------------
    public List<string> OrderCost { get; set; } = [];

    //------------- List From SpecialPickup ------------------------------
    public List<string> UsersThatHasSpecialCityCost { get; set; } = [];


}

public class CitySettingToAddDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal StandardShippingCost { get; set; }

    public decimal PickUpShippingCost { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? RegionId { get; set; }

}

public class CitySettingToUpdateDTO
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal StandardShippingCost { get; set; }

    public decimal PickUpShippingCost { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? RegionId { get; set; }

}