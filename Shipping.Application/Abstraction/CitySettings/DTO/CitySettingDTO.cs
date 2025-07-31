namespace Shipping.Application.Abstraction.CitySettings.DTO;

public class CitySettingBaseDTO
{
    public required string Name { get; set; }

    public decimal StandardShippingCost { get; set; }

    public decimal PickUpShippingCost { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? RegionId { get; set; }
}

public class CitySettingDTO : CitySettingBaseDTO
{
    public int Id { get; set; }

    public string? RegionName { get; set; }

    public List<string> Users { get; set; } = new();

    public List<string> OrderCost { get; set; } = new();

    public List<string> UsersThatHasSpecialCityCost { get; set; } = new();
}

public class CitySettingToAddDTO : CitySettingBaseDTO
{

}

public class CitySettingToUpdateDTO : CitySettingBaseDTO
{

}