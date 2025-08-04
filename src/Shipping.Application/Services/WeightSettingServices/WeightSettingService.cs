using AutoMapper;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.WeightSetting;
using Shipping.Application.Abstraction.WeightSetting.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.WeightSettingServices;
internal class WeightSettingService(ILogger<WeightSettingService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IWeightSettingService
{
    //GetAll WeightSetting
    public async Task<IEnumerable<WeightSettingDTO>> GetAllWeightSettingAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Retrieving all WeightSettings with pagination: {@PaginationParameters}", pramter);

        var weightSettings = await unitOfWork.GetRepository<WeightSetting, int>().GetAllAsync(pramter);

        return mapper.Map<IEnumerable<WeightSettingDTO>>(weightSettings);
    }

    //GetById WeightSetting
    public async Task<WeightSettingDTO> GetWeightSettingAsync(int id)
    {
        logger.LogInformation("Retrieving WeightSetting with ID: {Id}", id);

        var weightSetting = await unitOfWork.GetRepository<WeightSetting, int>().GetByIdAsync(id);

        if (weightSetting is null)
            throw new NotFoundException(nameof(WeightSetting), id.ToString());

        return mapper.Map<WeightSettingDTO>(weightSetting);
    }

    //Add WeightSetting
    public async Task<WeightSettingDTO> AddAsync(WeightSettingDTO DTO)
    {
        logger.LogInformation("Adding new WeightSetting: {@DTO}", DTO);

        var weightSettings = mapper.Map<WeightSetting>(DTO);

        await unitOfWork.GetRepository<WeightSetting, int>().AddAsync(weightSettings);

        await unitOfWork.CompleteAsync();

        return mapper.Map<WeightSettingDTO>(weightSettings);
    }

    //Update WeightSetting
    public async Task UpdateAsync(int id, WeightSettingDTO DTO)
    {
        logger.LogInformation("Updating WeightSetting with ID: {Id} using data: {@DTO}", id, DTO);

        var weightSettingRepo = unitOfWork.GetRepository<WeightSetting, int>();

        var existingWeightSetting = await weightSettingRepo.GetByIdAsync(id);

        if (existingWeightSetting is null)
            throw new NotFoundException(nameof(WeightSetting), id.ToString());

        mapper.Map(DTO, existingWeightSetting);

        weightSettingRepo.UpdateAsync(existingWeightSetting);

        await unitOfWork.CompleteAsync();
    }

    //Delete WeightSetting
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete WeightSetting with ID: {Id}", id);

        var weightSettingRepo = unitOfWork.GetRepository<WeightSetting, int>();

        var existingWeightSetting = await weightSettingRepo.GetByIdAsync(id);

        if (existingWeightSetting is null)
            throw new NotFoundException(nameof(WeightSetting), id.ToString());

        await weightSettingRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }
}
