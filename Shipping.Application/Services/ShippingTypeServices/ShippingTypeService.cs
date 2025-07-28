using AutoMapper;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Application.Abstraction.ShippingType.Serivce;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.ShippingTypeServices;
public class ShippingTypeService(IUnitOfWork _unitOfWork,
    IMapper _mapper) : IShippingTypeService
{
    //Add Shipping Type
    public async Task AddAsync(ShippingTypeDTO DTO)
    {
        await _unitOfWork.GetRepository<ShippingType, int>().AddAsync(_mapper.Map<ShippingType>(DTO));
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

    //Get All Shipping Type
    public async Task<IEnumerable<ShippingTypeDTO>> GetAllShippingTypeAsync(PaginationParameters pramter)
    {
        return _mapper.Map<IEnumerable<ShippingTypeDTO>>(await _unitOfWork.GetRepository<ShippingType, int>().GetAllAsync(pramter));
    }

    //GetById Shipping Type
    public async Task<ShippingTypeDTO> GetShippingTypeAsync(int id)
    {
        return _mapper.Map<ShippingTypeDTO>(await _unitOfWork.GetRepository<ShippingType, int>().GetByIdAsync(id));
    }

    //Update Shipping Type
    public async Task UpdateAsync(ShippingTypeDTO DTO)
    {
        var ShippingTypeRepo = _unitOfWork.GetRepository<ShippingType, int>();
        var existingShippingType = await ShippingTypeRepo.GetByIdAsync(DTO.Id);

        if (existingShippingType is null)
            throw new KeyNotFoundException($"Product with ID {DTO.Id} not found.");

        _mapper.Map(DTO, existingShippingType);

        ShippingTypeRepo.UpdateAsync(existingShippingType);

        await _unitOfWork.CompleteAsync();
    }
}
