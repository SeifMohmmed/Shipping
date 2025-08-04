using Shipping.Domain.Constants;
using Shipping.Domain.Enums;
using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.User.DTO;
public record AddCourierDTO
{
    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int BranchId { get; set; }

    [JsonIgnore]
    public string RoleName = UserRole.Courier;

    public DeductionTypes DeductionType { get; set; } = DeductionTypes.Fixed;

    public decimal DeductionCompanyFromOrder { get; set; } = 0;

    public List<CourierRegionDT0> SpecialCourierRegions { get; set; } = new();

}
public record CourierRegionDT0
{
    public int RegionId { get; set; }
    [JsonIgnore]
    public string CourierId { get; set; } = string.Empty;
}