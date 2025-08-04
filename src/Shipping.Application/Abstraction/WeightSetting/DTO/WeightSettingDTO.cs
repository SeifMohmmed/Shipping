using System.Text.Json.Serialization;

namespace Shipping.Application.Abstraction.WeightSetting.DTO;
public class WeightSettingBaseDTO
{
    public decimal MinWeight { get; set; }
    public decimal MaxWeight { get; set; }
    public decimal CostPerKg { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class WeightSettingDTO : WeightSettingBaseDTO
{
    [JsonIgnore]
    public int Id { get; set; }
}

public class UpdateWeightSettingDTO : WeightSettingBaseDTO
{

}
