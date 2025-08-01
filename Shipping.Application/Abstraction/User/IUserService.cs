using Shipping.Application.Abstraction.User.DTO;

namespace Shipping.Application.Abstraction.User;
public interface IUserService
{
    Task<AccountProfileDTO?> GetAccountProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task<string> AddEmployeeAsync(AddEmployeeDTO addEmployeeDTO, CancellationToken cancellationToken = default);
    Task<string> AddMerchantAsync(AddMerchantDTO addMerchantDTO, CancellationToken cancellationToken = default);
    Task<string> AddCourierAsync(AddCourierDTO addCourierDTO, CancellationToken cancellationToken = default);
}
