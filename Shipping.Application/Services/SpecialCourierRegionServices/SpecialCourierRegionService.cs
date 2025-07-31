using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.SpecialCourierRegion.DTO;
using Shipping.Application.Abstraction.SpecialCourierRegion.Serivce;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.SpecialCourierRegionServices;
public class SpecialCourierRegionService(IUnitOfWork unitOfWork,
    IMapper mapper) : ISpecialCourierRegionService
{
    //Get All SpecialCourierRegion
    public async Task<IEnumerable<SpecialCourierRegionDTO>> GetSpecialCourierRegionsAsync(PaginationParameters pramter)
    {
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
        var region = await unitOfWork.GetRepository<SpecialCourierRegion, int>().GetByIdAsync
            (id,
            include: q => q.Include(s => s.Region)
            .Include(s => s.Courier));

        return mapper.Map<SpecialCourierRegionDTO>(region);
    }

    //Add SpecialCourierRegion
    public async Task AddAsync(SpecialCourierRegionDTO DTO)
    {
        await unitOfWork.GetRepository<SpecialCourierRegion, int>().AddAsync(mapper.Map<SpecialCourierRegion>(DTO));
        await unitOfWork.CompleteAsync();
    }


    //Delete SpecialCourierRegion
    public async Task DeleteAsync(int id)
    {
        var SpecialCourierRegionRepo = unitOfWork.GetRepository<SpecialCourierRegion, int>();
        var existingSpecialCourierRegion = await SpecialCourierRegionRepo.GetByIdAsync(id);

        if (existingSpecialCourierRegion is null)
            throw new KeyNotFoundException($"SpecialCourierRegion with ID {id} not found.");

        await SpecialCourierRegionRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }

    //Update SpecialCourierRegion
    public async Task UpdateAsync(SpecialCourierRegionDTO DTO)
    {
        var SpecialCourierRegionRepo = unitOfWork.GetRepository<SpecialCourierRegion, int>();
        var existingSpecialCourierRegion = await SpecialCourierRegionRepo.GetByIdAsync(DTO.Id);

        if (existingSpecialCourierRegion is null)
            throw new KeyNotFoundException($"SpecialCourierRegion with ID {DTO.Id} not found.");

        mapper.Map(DTO, existingSpecialCourierRegion);

        SpecialCourierRegionRepo.UpdateAsync(existingSpecialCourierRegion);
        await unitOfWork.CompleteAsync();
    }

}
