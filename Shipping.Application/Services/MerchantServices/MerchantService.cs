using AutoMapper;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.People;
using Shipping.Application.Abstraction.People.DTOs;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.MerchantServices;
internal class MerchantService(ILogger<MerchantService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IMerchantService
{
    public async Task<IEnumerable<MerchantDTO>> GetMerchantAsync()
    {
        logger.LogInformation("Fetching all merchants...");

        var merchants = await unitOfWork.GetAllMerchantAsync().GetAllMerchantAsync();

        return mapper.Map<IEnumerable<MerchantDTO>>(merchants);
    }
}
