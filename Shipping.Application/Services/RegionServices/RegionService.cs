using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.Region;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.RegionServices;
internal class RegionService(ILogger<RegionService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRegionService
{
    //GetAll Regions
    public async Task<IEnumerable<RegionDTO>> GetRegionsAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Retrieving all regions with pagination: {@PaginationParameters}", pramter);

        var regions = await unitOfWork.GetRepository<Region, int>().GetAllAsync(pramter, include:
        p => p
        .Include(r => r.CitySettings));

        return mapper.Map<IEnumerable<RegionDTO>>(regions);
    }

    //GetById Region
    public async Task<RegionDTO> GetRegionAsync(int id)
    {
        logger.LogInformation("Retrieving region with ID: {RegionId}", id);

        var region = await unitOfWork.GetRepository<Region, int>().GetByIdAsync(id, include:
        p => p
        .Include(r => r.CitySettings));

        if (region is null)
            throw new NotFoundException(nameof(Region), id.ToString());

        return mapper.Map<RegionDTO>(region);
    }

    //Add Region
    public async Task<RegionDTO> AddAsync(RegionDTO DTO)
    {
        logger.LogInformation("Adding new region: {@RegionDTO}", DTO);

        var region = mapper.Map<Region>(DTO);

        await unitOfWork.GetRepository<Region, int>().AddAsync(region);

        await unitOfWork.CompleteAsync();

        return mapper.Map<RegionDTO>(region);
    }

    //Update Region
    public async Task UpdateAsync(int id, RegionDTO DTO)
    {
        logger.LogInformation("Updating region with ID: {RegionId} using data: {@RegionDTO}", id, DTO);

        var regionRepo = unitOfWork.GetRepository<Region, int>();

        var existingRegion = await regionRepo.GetByIdAsync(id, include:
            p => p
            .Include(r => r.CitySettings));

        if (existingRegion is null)
            throw new NotFoundException(nameof(Region), id.ToString());

        mapper.Map(DTO, existingRegion);

        regionRepo.UpdateAsync(existingRegion);

        await unitOfWork.CompleteAsync();
    }

    //Delete Region
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete region with ID: {RegionId}", id);

        var regionRepo = unitOfWork.GetRepository<Region, int>();

        var existingRegion = await regionRepo.GetByIdAsync(id);

        if (existingRegion is null)
            throw new NotFoundException(nameof(Region), id.ToString());

        await regionRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }

}
