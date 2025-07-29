using AutoMapper;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Application.Abstraction.Region.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.RegionServices;
internal class RegionService(IUnitOfWork unitOfWork,
    IMapper mapper) : IRegionService
{
    //GetById Region
    public async Task<RegionDTO> GetRegionAsync(int id)
    => mapper.Map<RegionDTO>(await unitOfWork.GetRepository<Region, int>().GetByIdAsync(id));

    //GetAll Regions
    public async Task<IEnumerable<RegionDTO>> GetRegionsAsync(PaginationParameters pramter)
    => mapper.Map<IEnumerable<RegionDTO>>(await unitOfWork.GetRepository<Region, int>().GetAllAsync(pramter));


    //Add Region
    public async Task AddAsync(RegionToAddDTO DTO)
    {
        await unitOfWork.GetRepository<Region, int>().AddAsync(mapper.Map<Region>(DTO));

        await unitOfWork.CompleteAsync();
    }


    //Delete Region
    public async Task DeleteAsync(int id)
    {
        var regionRepo = unitOfWork.GetRepository<Region, int>();
        var existingRegion = regionRepo.GetByIdAsync(id);

        if (existingRegion is null)
            throw new KeyNotFoundException($"Region with ID {id} not found.");

        await regionRepo.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }


    //Update Region
    public async Task UpdateAsync(RegionDTO DTO)
    {
        var regionRepo = unitOfWork.GetRepository<Region, int>();
        var existingRegion = await regionRepo.GetByIdAsync(DTO.Id);

        if (existingRegion is null)
            throw new KeyNotFoundException($"Region with ID {DTO.Id} not found.");

        mapper.Map(DTO, existingRegion);

        regionRepo.UpdateAsync(existingRegion);

        await unitOfWork.CompleteAsync();
    }
}
