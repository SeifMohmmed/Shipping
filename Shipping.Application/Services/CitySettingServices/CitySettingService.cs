using AutoMapper;
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
    => mapper.Map<IEnumerable<CitySettingDTO>>(await unitOfWork.GetRepository<CitySetting, int>().GetAllAsync(pramter));

    //GetById City
    public async Task<CitySettingDTO> GetCitySettingAsync(int id)
    => mapper.Map<CitySettingDTO>(await unitOfWork.GetRepository<CitySetting, int>().GetByIdAsync(id));

    //Add City
    public async Task AddAsync(CitySettingToAddDTO DTO)
    {
        await unitOfWork.GetRepository<CitySetting, int>().AddAsync(mapper.Map<CitySetting>(DTO));
        await unitOfWork.CompleteAsync();
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
    public async Task UpdateAsync(CitySettingToUpdateDTO DTO)
    {
        var citySettingRepo = unitOfWork.GetCityRepository();
        var existingCitySetting = await citySettingRepo.GetByIdAsync(DTO.Id);

        if (existingCitySetting is null)
            throw new KeyNotFoundException($"CitySetting with ID {DTO.Id} not found.");

        mapper.Map(DTO, existingCitySetting);

        citySettingRepo.UpdateAsync(existingCitySetting);

        await unitOfWork.CompleteAsync();
    }

    // Get city by governorate name
    public async Task<IEnumerable<CitySettingDTO>> GetCityByGovernorateName(int regionId)
    {
        var regionRepo = unitOfWork.GetRepository<Region, int>();
        var region = await regionRepo.GetByIdAsync(regionId);

        if (region is null)
            throw new KeyNotFoundException($"Region with ID {regionId} not found.");

        var cities = await unitOfWork.GetCityRepository().GetCityByGovernorateName(regionId);
        return mapper.Map<IEnumerable<CitySettingDTO>>(cities);
    }
}
