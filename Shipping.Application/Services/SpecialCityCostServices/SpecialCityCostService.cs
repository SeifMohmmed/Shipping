using AutoMapper;
using Shipping.Application.Abstraction.SpecialCityCost.DTO;
using Shipping.Application.Abstraction.SpecialCityCost.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.SpecialCityCostServices;
internal class SpecialCityCostService(IUnitOfWork unitOfWork,
    IMapper mapper) : ISpecialCityCostService
{

    //GetAll SpecialCityCost
    public async Task<IEnumerable<SpecialCityCostDTO>> GetAllSpecialCityCostAsync(PaginationParameters pramter)
     => mapper.Map<IEnumerable<SpecialCityCostDTO>>(await unitOfWork.GetRepository<SpecialCityCost, int>().GetAllAsync(pramter));


    //GetById SpecialCityCost
    public async Task<SpecialCityCostDTO> GetSpecialCityCostAsync(int id)
    => mapper.Map<SpecialCityCostDTO>(await unitOfWork.GetRepository<SpecialCityCost, int>().GetByIdAsync(id));


    //Add SpecialCityCost
    public async Task AddAsync(SpecialCityCostDTO DTO)
    {
        var cityCost =
            await unitOfWork.GetRepository<SpecialCityCost, int>().GetByIdAsync(DTO.Id);

        if (cityCost is null)
            throw new KeyNotFoundException($"SpecialCityCost with ID {DTO.CitySettingId} does not exist.");

        await unitOfWork.GetRepository<SpecialCityCost, int>().AddAsync(mapper.Map<SpecialCityCost>(DTO));

        await unitOfWork.CompleteAsync();
    }


    //Delete SpecialCityCost
    public async Task DeleteAsync(int id)
    {
        var specialCityRepo = unitOfWork.GetRepository<SpecialCityCost, int>();

        var existingSpecialCityCost = await specialCityRepo.GetByIdAsync(id);

        if (existingSpecialCityCost is null)
            throw new KeyNotFoundException($"SpecialCityCost with ID {id} does not exist.");

        await specialCityRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }


    //Update SpecialCityCost
    public async Task UpdateAsync(SpecialCityCostDTO DTO)
    {
        var specialCityRepo = unitOfWork.GetRepository<SpecialCityCost, int>();

        var existingSpecialCityCost = await specialCityRepo.GetByIdAsync(DTO.Id);

        if (existingSpecialCityCost is null)
            throw new KeyNotFoundException($"SpecialCityCost with ID {DTO.Id} does not exist.");

        mapper.Map(DTO, existingSpecialCityCost);

        specialCityRepo.UpdateAsync(existingSpecialCityCost);

        await unitOfWork.CompleteAsync();
    }


}
