using Shipping.Domain.Helpers;
using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.User.DTO;
public record AddMerchantDTO
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public int BranchId { get; set; }

    public int RegionId { get; set; }

    public int CityId { get; set; }

    public string StoreName { get; set; } = string.Empty;

    [JsonIgnore]
    public string RoleName { get; set; } = DefaultRole.Merchant;

    public List<SpecialCityCostDT0>? SpecialCityCosts { get; set; } = new();

}
public record SpecialCityCostDT0
{
    public decimal Price { get; set; }
    public int CitySettingId { get; set; }
    [JsonIgnore]
    public string MerchantId { get; set; } = string.Empty;
}