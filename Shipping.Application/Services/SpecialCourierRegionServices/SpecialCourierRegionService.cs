using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.SpecialCourierRegion.DTO;
using Shipping.Application.Abstraction.SpecialCourierRegion.Serivce;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.SpecialCourierRegionServices;
public class SpecialCourierRegionService(ILogger<SpecialCourierRegionService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ISpecialCourierRegionService
{
    //Get All SpecialCourierRegion
    public async Task<IEnumerable<SpecialCourierRegionDTO>> GetSpecialCourierRegionsAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Retrieving all SpecialCourierRegions with pagination: {@PaginationParameters}", pramter);

        var regions = await unitOfWork
            .GetRepository<SpecialCourierRegion, int>().GetAllAsync
            (
              pramter,
              include: q => q
              .Include(s => s.Region)
              .Include(s => s.Courier)
            );

        return mapper.Map<IEnumerable<SpecialCourierRegionDTO>>(regions);

    }

    //GetById SpecialCourierRegion
    public async Task<SpecialCourierRegionDTO> GetSpecialCourierRegionAsync(int id)
    {
        logger.LogInformation("Retrieving SpecialCourierRegion with ID: {Id}", id);

        var region = await unitOfWork.GetRepository<SpecialCourierRegion, int>().GetByIdAsync
            (id,
            include: q => q.Include(s => s.Region)
            .Include(s => s.Courier));

        if (region is null)
            throw new NotFoundException(nameof(SpecialCourierRegion), id.ToString());

        return mapper.Map<SpecialCourierRegionDTO>(region);
    }

    //Add SpecialCourierRegion
    public async Task AddAsync(SpecialCourierRegionDTO DTO)
    {
        logger.LogInformation("Adding new SpecialCourierRegion: {@DTO}", DTO);

        await unitOfWork.GetRepository<SpecialCourierRegion, int>().AddAsync(mapper.Map<SpecialCourierRegion>(DTO));
        await unitOfWork.CompleteAsync();
    }


    //Update SpecialCourierRegion
    public async Task UpdateAsync(SpecialCourierRegionDTO DTO)
    {
        logger.LogInformation("Updating SpecialCourierRegion with ID: {Id} using data: {@DTO}", DTO.Id, DTO);

        var SpecialCourierRegionRepo = unitOfWork.GetRepository<SpecialCourierRegion, int>();
        var existingSpecialCourierRegion = await SpecialCourierRegionRepo.GetByIdAsync(DTO.Id);

        if (existingSpecialCourierRegion is null)
            throw new NotFoundException(nameof(SpecialCourierRegion), DTO.Id.ToString());

        mapper.Map(DTO, existingSpecialCourierRegion);

        SpecialCourierRegionRepo.UpdateAsync(existingSpecialCourierRegion);
        await unitOfWork.CompleteAsync();
    }


    //Delete SpecialCourierRegion
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete SpecialCourierRegion with ID: {Id}", id);

        var SpecialCourierRegionRepo = unitOfWork.GetRepository<SpecialCourierRegion, int>();
        var existingSpecialCourierRegion = await SpecialCourierRegionRepo.GetByIdAsync(id);

        if (existingSpecialCourierRegion is null)
            throw new NotFoundException(nameof(SpecialCourierRegion), id.ToString());

        await SpecialCourierRegionRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }

}
