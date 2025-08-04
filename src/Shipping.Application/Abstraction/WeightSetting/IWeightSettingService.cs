using Shipping.Application.Abstraction.WeightSetting.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.WeightSetting;
public interface IWeightSettingService
{
    Task<IEnumerable<WeightSettingDTO>> GetAllWeightSettingAsync(PaginationParameters pramter);
    Task<WeightSettingDTO> GetWeightSettingAsync(int id);
    Task<WeightSettingDTO> AddAsync(WeightSettingDTO DTO);
    Task UpdateAsync(int id, WeightSettingDTO DTO);
    Task DeleteAsync(int id);
}
