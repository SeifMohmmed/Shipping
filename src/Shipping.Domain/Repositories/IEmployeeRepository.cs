using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.Domain.Repositories;
public interface IEmployeeRepository
{
    // This Method Is Used To Get All Employees With pramter (Pagination)
    Task<IEnumerable<ApplicationUser>> GetAllEmployeesAsync(PaginationParameters pramter);
}
