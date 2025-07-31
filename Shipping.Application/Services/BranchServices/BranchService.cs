using AutoMapper;
using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Application.Abstraction.Branch.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.BranchServices;
public class BranchService(IUnitOfWork unitOfWork,
    IMapper mapper) : IBranchService
{
    public async Task<BranchDTO> GetBranchAsync(int id)
    => mapper.Map<BranchDTO>(await unitOfWork.GetRepository<Branch, int>().GetByIdAsync(id));

    public async Task<IEnumerable<BranchDTO>> GetBranchesAsync(PaginationParameters pramter)
    => mapper.Map<IEnumerable<BranchDTO>>(await unitOfWork.GetRepository<Branch, int>().GetAllAsync(pramter));


    public async Task<BranchDTO> AddAsync(BranchToAddDTO DTO)
    {
        var entity = mapper.Map<Branch>(DTO);

        await unitOfWork.GetRepository<Branch, int>().AddAsync(entity);

        await unitOfWork.CompleteAsync();

        return mapper.Map<BranchDTO>(entity);
    }


    public async Task DeleteAsync(int id)
    {
        var branchRepo = unitOfWork.GetRepository<Branch, int>();
        var existingBranch = await branchRepo.GetByIdAsync(id);

        if (existingBranch is null)
            throw new KeyNotFoundException($"Branch with ID {id} not found.");

        await branchRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }


    public async Task UpdateAsync(int id, BranchToUpdateDTO DTO)
    {
        var branchRepo = unitOfWork.GetRepository<Branch, int>();

        var existingBranch = await branchRepo.GetByIdAsync(id);

        if (existingBranch is null)
            throw new KeyNotFoundException($"Branch with ID {id} not found.");

        mapper.Map(DTO, existingBranch);

        branchRepo.UpdateAsync(existingBranch);

        await unitOfWork.CompleteAsync();
    }

}
