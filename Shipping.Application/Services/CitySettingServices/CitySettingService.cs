using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Application.Abstraction.CitySettings.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.CitySettingServices;
internal class CitySettingService(ILogger<CitySettingService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICitySettingService
{
    //GetAll Cities
    public async Task<IEnumerable<CitySettingDTO>> GetAllCitySettingAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Getting all CitySetting with pagination {@Pagination}", pramter);

        var entity = await unitOfWork.GetRepository<CitySetting, int>()
            .GetAllAsync(pramter,
            p => p.Include(c => c.Region)
            .Include(c => c.Users)
            .Include(c => c.Orders)
            .Include(c => c.SpecialPickups)
                .ThenInclude(sp => sp.Merchant)
            );

        return mapper.Map<IEnumerable<CitySettingDTO>>(entity);
    }

    //GetById City
    public async Task<CitySettingDTO> GetCitySettingAsync(int id)
    {
        logger.LogInformation("Getting CitySetting by {CitySettingId}", id);

        var citySetting = await unitOfWork.GetRepository<CitySetting, int>()
            .GetByIdAsync(id,
            p => p.Include(c => c.Region)
            .Include(c => c.Users)
            .Include(c => c.Orders)
            .Include(c => c.SpecialPickups)
                .ThenInclude(sp => sp.Merchant)
            );

        if (citySetting is null)
            throw new NotFoundException(nameof(CitySetting), id.ToString());

        return mapper.Map<CitySettingDTO>(citySetting);
    }

    //Add City
    public async Task<CitySettingDTO> AddAsync(CitySettingToAddDTO DTO)
    {
        logger.LogInformation("Adding new CitySetting {@citySettingToAdd}", DTO);

        var citySetting = mapper.Map<CitySetting>(DTO);

        await unitOfWork.GetRepository<CitySetting, int>().AddAsync(citySetting);

        await unitOfWork.CompleteAsync();

        return mapper.Map<CitySettingDTO>(citySetting);
    }

    //Update City
    public async Task UpdateAsync(int id, CitySettingToUpdateDTO DTO)
    {
        logger.LogInformation("Updating CitySetting {CitySettingId} with data {@UpdateDTO}", id, DTO);

        var citySettingRepo = unitOfWork.GetCityRepository();
        var existingCitySetting = await citySettingRepo.GetByIdAsync(id);

        if (existingCitySetting is null)
            throw new NotFoundException(nameof(CitySetting), id.ToString());

        mapper.Map(DTO, existingCitySetting);

        citySettingRepo.UpdateAsync(existingCitySetting);

        await unitOfWork.CompleteAsync();
    }

    //Delete City
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete CitySetting {CitySettingId}", id);

        var citySettingRepo = unitOfWork.GetCityRepository();

        var city = await citySettingRepo.GetByIdAsync(id);

        if (city is null)
            throw new NotFoundException(nameof(CitySetting), id.ToString());

        await citySettingRepo.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }


    // Get city by governorate name
    public async Task<IEnumerable<CitySettingDTO>> GetCitiesByRegionId(int regionId)
    {
        logger.LogInformation("Fetching region with ID: {RegionId}", regionId);

        var regionRepo = unitOfWork.GetRepository<Region, int>();
        var region = await regionRepo.GetByIdAsync(regionId,
            p => p
                .Include(c => c.Users)
                .Include(c => c.Orders)
                .Include(c => c.SpecialCourierRegion)
                    .ThenInclude(sp => sp.Courier)
                .Include(c => c.CitySettings)
        );

        if (region is null)
            throw new NotFoundException(nameof(Region), regionId.ToString());

        var cities = await unitOfWork.GetCityRepository().GetCityByGovernorateName(regionId);
        return mapper.Map<IEnumerable<CitySettingDTO>>(cities);
    }
}
