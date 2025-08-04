using AutoMapper;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Application.Abstraction.Branch.Service;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.BranchServices;
public class BranchService(ILogger<BranchService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IBranchService
{
    //Get all Branches
    public async Task<IEnumerable<BranchDTO>> GetBranchesAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Getting all branches with pagination {@Pagination}", pramter);

        var branches = await unitOfWork.GetRepository<Branch, int>().GetAllAsync(pramter);

        return mapper.Map<IEnumerable<BranchDTO>>(branches);
    }

    //GetById Branch
    public async Task<BranchDTO> GetBranchAsync(int id)
    {
        logger.LogInformation("Getting branch by {BranchId}", id);

        var branch = await unitOfWork.GetRepository<Branch, int>().GetByIdAsync(id);

        if (branch is null)
            throw new NotFoundException(nameof(Branch), id.ToString());

        return mapper.Map<BranchDTO>(branch);
    }

    //Add Branch
    public async Task<BranchDTO> AddAsync(BranchToAddDTO DTO)
    {
        logger.LogInformation("Adding new branch {@BranchToAdd}", DTO);

        var entity = mapper.Map<Branch>(DTO);

        await unitOfWork.GetRepository<Branch, int>().AddAsync(entity);

        await unitOfWork.CompleteAsync();

        return mapper.Map<BranchDTO>(entity);
    }

    //Update Branch
    public async Task UpdateAsync(int id, BranchToUpdateDTO DTO)
    {
        logger.LogInformation("Updating branch {BranchId} with data {@UpdateDTO}", id, DTO);

        var branchRepo = unitOfWork.GetRepository<Branch, int>();

        var existingBranch = await branchRepo.GetByIdAsync(id);

        if (existingBranch is null)
            throw new NotFoundException(nameof(Branch), id.ToString());

        mapper.Map(DTO, existingBranch);

        branchRepo.UpdateAsync(existingBranch);

        await unitOfWork.CompleteAsync();
    }

    //Delete Branch
    public async Task DeleteAsync(int id)
    {
        logger.LogInformation("Attempting to delete branch {BranchId}", id);

        var branchRepo = unitOfWork.GetRepository<Branch, int>();
        var existingBranch = await branchRepo.GetByIdAsync(id);

        if (existingBranch is null)
            throw new NotFoundException(nameof(Branch), id.ToString());

        await branchRepo.DeleteAsync(id);

        await unitOfWork.CompleteAsync();
    }



}
