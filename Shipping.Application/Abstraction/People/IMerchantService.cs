using Shipping.Application.Abstraction.People.DTOs;

namespace Shipping.Application.Abstraction.People;
public interface IMerchantService
{
    Task<IEnumerable<MerchantDTO>> GetMerchantAsync();

}
