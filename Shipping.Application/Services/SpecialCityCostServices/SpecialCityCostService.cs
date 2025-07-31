using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    {
        var specialCityCosts = await unitOfWork.GetRepository<SpecialCityCost, int>().GetAllAsync(pramter,
            include: p => p
            .Include(c => c.CitySetting)
            .Include(c => c.Merchant));

        return mapper.Map<IEnumerable<SpecialCityCostDTO>>(specialCityCosts);
    }

    //GetById SpecialCityCost
    public async Task<SpecialCityCostDTO> GetSpecialCityCostAsync(int id)
    {
        var specialCityCost = await unitOfWork.GetRepository<SpecialCityCost, int>().GetByIdAsync(id, include:
            p => p
            .Include(c => c.CitySetting)
            .Include(c => c.Merchant));

        return mapper.Map<SpecialCityCostDTO>(specialCityCost);
    }

    //Add SpecialCityCost
    public async Task<SpecialCityAddDTO> AddAsync(SpecialCityAddDTO DTO)
    {
        var specialCityCost = mapper.Map<SpecialCityCost>(DTO);

        await unitOfWork.GetRepository<SpecialCityCost, int>().AddAsync(specialCityCost);

        await unitOfWork.CompleteAsync();

        return mapper.Map<SpecialCityAddDTO>(specialCityCost);
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
    public async Task UpdateAsync(int id, SpecialCityUpdateDTO DTO)
    {
        var specialCityRepo = unitOfWork.GetRepository<SpecialCityCost, int>();

        var existingSpecialCityCost = await specialCityRepo.GetByIdAsync(id);

        if (existingSpecialCityCost is null)
            throw new KeyNotFoundException($"SpecialCityCost with ID {id} does not exist.");

        mapper.Map(DTO, existingSpecialCityCost);

        specialCityRepo.UpdateAsync(existingSpecialCityCost);

        await unitOfWork.CompleteAsync();

    }


}
