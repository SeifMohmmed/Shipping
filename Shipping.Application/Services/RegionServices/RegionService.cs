using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.Region;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.RegionServices;
internal class RegionService(IUnitOfWork unitOfWork,
    IMapper mapper) : IRegionService
{
    //GetAll Regions
    public async Task<IEnumerable<RegionDTO>> GetRegionsAsync(PaginationParameters pramter)
    => mapper.Map<IEnumerable<RegionDTO>>(await unitOfWork.GetRepository<Region, int>().GetAllAsync(pramter, include:
        p => p
        .Include(r => r.CitySettings)));

    //GetById Region
    public async Task<RegionDTO> GetRegionAsync(int id)
    => mapper.Map<RegionDTO>(await unitOfWork.GetRepository<Region, int>().GetByIdAsync(id, include:
        p => p
        .Include(r => r.CitySettings)));


    //Add Region
    public async Task<RegionDTO> AddAsync(RegionDTO DTO)
    {
        var region = mapper.Map<Region>(DTO);

        await unitOfWork.GetRepository<Region, int>().AddAsync(region);

        await unitOfWork.CompleteAsync();

        return mapper.Map<RegionDTO>(region);
    }

    //Update Region
    public async Task UpdateAsync(int id, RegionDTO DTO)
    {
        var regionRepo = unitOfWork.GetRepository<Region, int>();

        var existingRegion = await regionRepo.GetByIdAsync(id, include:
            p => p
            .Include(r => r.CitySettings));

        if (existingRegion is null)
            throw new KeyNotFoundException($"Region with ID {id} not found.");

        mapper.Map(DTO, existingRegion);

        regionRepo.UpdateAsync(existingRegion);

        await unitOfWork.CompleteAsync();
    }

    //Delete Region
    public async Task DeleteAsync(int id)
    {
        var regionRepo = unitOfWork.GetRepository<Region, int>();

        var existingRegion = await regionRepo.GetByIdAsync(id);

        if (existingRegion is null)
            throw new KeyNotFoundException($"Region with ID {id} not found.");

        await regionRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }

}
