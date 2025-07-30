using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.SpecialCityCost.DTO;
public class SpecialCityCostDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public decimal Price { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string MerchantId { get; set; } = string.Empty;

    public string? MerchantName { get; set; } = string.Empty;

    public int CitySettingId { get; set; }

    public string CitySettingName { get; set; } = string.Empty;

}
