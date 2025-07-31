using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.Branch.Service;
public interface IBranchService
{
    Task<IEnumerable<BranchDTO>> GetBranchesAsync(PaginationParameters pramter);
    Task<BranchDTO> GetBranchAsync(int id);
    Task<BranchDTO> AddAsync(BranchToAddDTO DTO);
    Task UpdateAsync(int id, BranchToUpdateDTO DTO);
    Task DeleteAsync(int id);
}
