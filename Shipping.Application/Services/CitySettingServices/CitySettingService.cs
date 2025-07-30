using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Application.Abstraction.CitySettings.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.CitySettingServices;
internal class CitySettingService(IUnitOfWork unitOfWork,
    IMapper mapper) : ICitySettingService
{
    //GetAll Cities
    public async Task<IEnumerable<CitySettingDTO>> GetAllCitySettingAsync(PaginationParameters pramter)
    {
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
        var entity = await unitOfWork.GetRepository<CitySetting, int>()
            .GetByIdAsync(id,
            p => p.Include(c => c.Region)
            .Include(c => c.Users)
            .Include(c => c.Orders)
            .Include(c => c.SpecialPickups)
                .ThenInclude(sp => sp.Merchant)
            );

        return mapper.Map<CitySettingDTO>(entity);
    }

    //Add City
    public async Task<CitySettingDTO> AddAsync(CitySettingToAddDTO DTO)
    {
        var entity = mapper.Map<CitySetting>(DTO);

        await unitOfWork.GetRepository<CitySetting, int>().AddAsync(entity);

        await unitOfWork.CompleteAsync();

        return mapper.Map<CitySettingDTO>(entity);
    }

    //Delete City
    public async Task DeleteAsync(int id)
    {
        var citySettingRepo = unitOfWork.GetCityRepository();

        var city = await citySettingRepo.GetByIdAsync(id);

        if (city is null)
            throw new KeyNotFoundException($"CitySetting with ID {id} not found.");

        await citySettingRepo.DeleteAsync(id);
        await unitOfWork.CompleteAsync();
    }

    //Update City
    public async Task UpdateAsync(int id, CitySettingToUpdateDTO DTO)
    {
        var citySettingRepo = unitOfWork.GetCityRepository();
        var existingCitySetting = await citySettingRepo.GetByIdAsync(id);

        if (existingCitySetting is null)
            throw new KeyNotFoundException($"CitySetting with ID {id} not found.");

        mapper.Map(DTO, existingCitySetting);

        citySettingRepo.UpdateAsync(existingCitySetting);

        await unitOfWork.CompleteAsync();
    }

    // Get city by governorate name
    public async Task<IEnumerable<CitySettingDTO>> GetCitiesByRegionId(int regionId)
    {
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
            throw new KeyNotFoundException($"Region with ID {regionId} not found.");

        var cities = await unitOfWork.GetCityRepository().GetCityByGovernorateName(regionId);
        return mapper.Map<IEnumerable<CitySettingDTO>>(cities);
    }
}
