using Shipping.Application.Abstraction.Merchant.DTO;

namespace Shipping.Application.Abstraction.Merchant;
public interface IMerchantService
{
    Task<IEnumerable<MerchantDTO>> GetMerchantAsync();

}
