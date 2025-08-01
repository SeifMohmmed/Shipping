using Shipping.Domain.Entities;

namespace Shipping.Domain.Repositories;
public interface IMerchantRepository
{
    Task<IEnumerable<ApplicationUser>> GetAllMerchantAsync();
}
