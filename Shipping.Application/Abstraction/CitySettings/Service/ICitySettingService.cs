﻿using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.CitySettings.Service;
public interface ICitySettingService
{
    Task<IEnumerable<CitySettingDTO>> GetAllCitySettingAsync(PaginationParameters pramter);
    Task<CitySettingDTO> GetCitySettingAsync(int id);
    Task<CitySettingDTO> AddAsync(CitySettingToAddDTO DTO);
    Task UpdateAsync(int id, CitySettingToUpdateDTO DTO);
    Task DeleteAsync(int id);
    Task<IEnumerable<CitySettingDTO>> GetCitiesByRegionId(int regionId);

}
