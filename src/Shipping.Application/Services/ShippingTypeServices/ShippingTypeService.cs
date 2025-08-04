using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Application.Abstraction.ShippingType.Serivce;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.ShippingTypeServices;
public class ShippingTypeService(ILogger<ShippingTypeService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IShippingTypeService
{
    //Get All Shipping Type
    public async Task<IEnumerable<ShippingTypeDTO>> GetAllShippingTypeAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Retrieving all shipping types with pagination: {@PaginationParameters}", pramter);

        var shippingTypes = await unitOfWork.GetRepository<ShippingType, int>().GetAllAsync(pramter,
            include: p => p
            .Include(s => s.Orders));

        return mapper.Map<IEnumerable<ShippingTypeDTO>>(shippingTypes);
    }

    //GetById Shipping Type
    public async Task<ShippingTypeDTO> GetShippingTypeAsync(int id)
    {
        logger.LogInformation("Retrieving shipping type with ID: {ShippingTypeId}", id);

        var shippingType = await unitOfWork.GetRepository<ShippingType, int>().GetByIdAsync(id,
            include: p => p
            .Include(s => s.Orders));

        if (shippingType is null)
            throw new NotFoundException(nameof(ShippingType), id.ToString());

        return mapper.Map<ShippingTypeDTO>(shippingType);
    }


    //Add Shipping Type
    public async Task AddAsync(ShippingTypeAddDTO DTO)
    {
        logger.LogInformation("Adding new shipping type: {@ShippingTypeAddDTO}", DTO);

        await unitOfWork.GetRepository<ShippingType, int>().AddAsync(mapper.Map<ShippingType>(DTO));
        await unitOfWork.CompleteAsync();
    }

    //Update Shipping Type
    public async Task UpdateAsync(int id, ShippingTypeUpdateDTO DTO)
    {
        logger.LogInformation("Updating shipping type with ID: {ShippingTypeId} using data: {@ShippingTypeUpdateDTO}", id, DTO);

        var ShippingTypeRepo = unitOfWork.GetRepository<ShippingType, int>();

        var existingShippingType = await ShippingTypeRepo.GetByIdAsync(id, include:
            p => p
            .Include(s => s.Orders));

        if (existingShippingType is null)
            throw new NotFoundException(nameof(ShippingType), id.ToString());

        mapper.Map(DTO, existingShippingType);

        ShippingTypeRepo.UpdateAsync(existingShippingType);

        await unitOfWork.CompleteAsync();
    }

    //Delete ShippingType
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete shipping type with ID: {ShippingTypeId}", id);

        var ShippingTypeRepo = unitOfWork.GetRepository<ShippingType, int>();
        var existingShippingType = await ShippingTypeRepo.GetByIdAsync(id);

        if (existingShippingType is null)
            throw new NotFoundException(nameof(ShippingType), id.ToString());

        await ShippingTypeRepo.DeleteAsync(id);
        await unitOfWork.CompleteAsync();

    }
}
