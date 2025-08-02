using AutoMapper;
using Shipping.Application.Abstraction.Merchant;
using Shipping.Application.Abstraction.Merchant.DTO;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.MerchantServices;
internal class MerchantService(IUnitOfWork unitOfWork,
    IMapper mapper) : IMerchantService
{
    public async Task<IEnumerable<MerchantDTO>> GetMerchantAsync()
    {
        return mapper.Map<IEnumerable<MerchantDTO>>(await unitOfWork.GetAllMerchantAsync().GetAllMerchantAsync());
    }
}
