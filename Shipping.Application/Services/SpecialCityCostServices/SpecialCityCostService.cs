using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.SpecialCityCost.DTO;
using Shipping.Application.Abstraction.SpecialCityCost.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.SpecialCityCostServices;
internal class SpecialCityCostService(ILogger<SpecialCityCostService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ISpecialCityCostService
{
    //GetAll SpecialCityCost
    public async Task<IEnumerable<SpecialCityCostDTO>> GetAllSpecialCityCostAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Retrieving all SpecialCityCosts with pagination: {@PaginationParameters}", pramter);

        var specialCityCosts = await unitOfWork.GetRepository<SpecialCityCost, int>().GetAllAsync(pramter,
            include: p => p
            .Include(c => c.CitySetting)
            .Include(c => c.Merchant));

        return mapper.Map<IEnumerable<SpecialCityCostDTO>>(specialCityCosts);
    }


    //GetById SpecialCityCost
    public async Task<SpecialCityCostDTO> GetSpecialCityCostAsync(int id)
    {
        logger.LogInformation("Retrieving SpecialCityCost with ID: {Id}", id);

        var specialCityCost = await unitOfWork.GetRepository<SpecialCityCost, int>().GetByIdAsync(id, include:
            p => p
            .Include(c => c.CitySetting)
            .Include(c => c.Merchant));

        if (specialCityCost is null)
            throw new NotFoundException(nameof(SpecialCityCost), id.ToString());

        return mapper.Map<SpecialCityCostDTO>(specialCityCost);
    }


    //Add SpecialCityCost
    public async Task<SpecialCityAddDTO> AddAsync(SpecialCityAddDTO DTO)
    {
        logger.LogInformation("Adding new SpecialCityCost: {@DTO}", DTO);

        var specialCityCost = mapper.Map<SpecialCityCost>(DTO);

        await unitOfWork.GetRepository<SpecialCityCost, int>().AddAsync(specialCityCost);

        await unitOfWork.CompleteAsync();

        return mapper.Map<SpecialCityAddDTO>(specialCityCost);
    }


    //Update SpecialCityCost
    public async Task UpdateAsync(int id, SpecialCityUpdateDTO DTO)
    {
        logger.LogInformation("Updating SpecialCityCost with ID: {Id} using data: {@DTO}", id, DTO);

        var specialCityRepo = unitOfWork.GetRepository<SpecialCityCost, int>();

        var existingSpecialCityCost = await specialCityRepo.GetByIdAsync(id);

        if (existingSpecialCityCost is null)
            throw new KeyNotFoundException($"SpecialCityCost with ID {id} does not exist.");

        mapper.Map(DTO, existingSpecialCityCost);

        specialCityRepo.UpdateAsync(existingSpecialCityCost);

        await unitOfWork.CompleteAsync();

    }


    //Delete SpecialCityCost
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete SpecialCityCost with ID: {Id}", id);

        var specialCityRepo = unitOfWork.GetRepository<SpecialCityCost, int>();

        var existingSpecialCityCost = await specialCityRepo.GetByIdAsync(id);

        if (existingSpecialCityCost is null)
            throw new NotFoundException(nameof(SpecialCityCost), id.ToString());

        await specialCityRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }
}
