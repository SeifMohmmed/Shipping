using Shipping.Domain.Entities;

namespace Shipping.Domain.Repositories;
public interface IWeightSettingRepository : IGenericRepository<WeightSetting, int>
{
    // This Method To Get All WeightSetting
    Task<IEnumerable<WeightSetting>> GetAllWeightSetting();

}
