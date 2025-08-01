using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.Employee;
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(PaginationParameters pramter);
}
