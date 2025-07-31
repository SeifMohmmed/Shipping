using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Application.Abstraction.ShippingType.Serivce;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.ShippingTypeServices;
public class ShippingTypeService(IUnitOfWork _unitOfWork,
    IMapper _mapper) : IShippingTypeService
{
    //Get All Shipping Type
    public async Task<IEnumerable<ShippingTypeDTO>> GetAllShippingTypeAsync(PaginationParameters pramter)
    {
        var shippingTypes = await _unitOfWork.GetRepository<ShippingType, int>().GetAllAsync(pramter,
            include: p => p
            .Include(s => s.Orders));

        return _mapper.Map<IEnumerable<ShippingTypeDTO>>(shippingTypes);
    }

    //GetById Shipping Type
    public async Task<ShippingTypeDTO> GetShippingTypeAsync(int id)
    {
        var shippingType = await _unitOfWork.GetRepository<ShippingType, int>().GetByIdAsync(id,
            include: p => p
            .Include(s => s.Orders));

        return _mapper.Map<ShippingTypeDTO>(shippingType);
    }


    //Add Shipping Type
    public async Task AddAsync(ShippingTypeAddDTO DTO)
    {
        await _unitOfWork.GetRepository<ShippingType, int>().AddAsync(_mapper.Map<ShippingType>(DTO));
        await _unitOfWork.CompleteAsync();
    }

    //Update Shipping Type
    public async Task UpdateAsync(int id, ShippingTypeUpdateDTO DTO)
    {
        var ShippingTypeRepo = _unitOfWork.GetRepository<ShippingType, int>();

        var existingShippingType = await ShippingTypeRepo.GetByIdAsync(id, include:
            p => p
            .Include(s => s.Orders));

        if (existingShippingType is null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        _mapper.Map(DTO, existingShippingType);

        ShippingTypeRepo.UpdateAsync(existingShippingType);

        await _unitOfWork.CompleteAsync();
    }

    //Delete ShippingType
    public async Task DeleteAsync(int id)
    {
        var ShippingTypeRepo = _unitOfWork.GetRepository<ShippingType, int>();
        var existingShippingType = await ShippingTypeRepo.GetByIdAsync(id);

        if (existingShippingType is null)
            throw new KeyNotFoundException($"ShippingType with ID {id} not found.");

        await ShippingTypeRepo.DeleteAsync(id);
        await _unitOfWork.CompleteAsync();

    }
}
