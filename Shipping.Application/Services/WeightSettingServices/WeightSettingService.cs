using AutoMapper;
using Shipping.Application.Abstraction.WeightSetting;
using Shipping.Application.Abstraction.WeightSetting.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.WeightSettingServices;
internal class WeightSettingService(IUnitOfWork unitOfWork,
    IMapper mapper) : IWeightSettingService
{
    //GetAll WeightSetting
    public async Task<IEnumerable<WeightSettingDTO>> GetAllWeightSettingAsync(PaginationParameters pramter)
    => mapper.Map<IEnumerable<WeightSettingDTO>>(await unitOfWork.GetRepository<WeightSetting, int>().GetAllAsync(pramter));

    //GetById WeightSetting
    public async Task<WeightSettingDTO> GetWeightSettingAsync(int id)
    => mapper.Map<WeightSettingDTO>(await unitOfWork.GetRepository<WeightSetting, int>().GetByIdAsync(id));

    //Add WeightSetting
    public async Task<WeightSettingDTO> AddAsync(WeightSettingDTO DTO)
    {
        var weightSettings = mapper.Map<WeightSetting>(DTO);

        await unitOfWork.GetRepository<WeightSetting, int>().AddAsync(weightSettings);

        await unitOfWork.CompleteAsync();

        return mapper.Map<WeightSettingDTO>(weightSettings);
    }

    //Update WeightSetting
    public async Task UpdateAsync(int id, WeightSettingDTO DTO)
    {
        var weightSettingRepo = unitOfWork.GetRepository<WeightSetting, int>();

        var existingWeightSetting = await weightSettingRepo.GetByIdAsync(id);

        if (existingWeightSetting is null)
            throw new KeyNotFoundException($"WeightSetting with ID {id} not found.");

        mapper.Map(DTO, existingWeightSetting);

        weightSettingRepo.UpdateAsync(existingWeightSetting);

        await unitOfWork.CompleteAsync();
    }

    //Delete WeightSetting
    public async Task DeleteAsync(int id)
    {
        var weightSettingRepo = unitOfWork.GetRepository<WeightSetting, int>();

        var existingWeightSettingDTO = await weightSettingRepo.GetByIdAsync(id);

        if (existingWeightSettingDTO is null)
            throw new KeyNotFoundException($"WeightSetting with ID {id} not found.");

        await weightSettingRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }
}
