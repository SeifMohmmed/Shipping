using AutoMapper;
using Shipping.Application.Abstraction.Employee;
using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.EmployeeServices;
internal class EmployeeService(IUnitOfWork unitOfWork, IMapper mapper) : IEmployeeService
{
    public async Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(PaginationParameters pramter)
    => mapper.Map<IEnumerable<EmployeeDTO>>(await unitOfWork.GetAllEmployeesAsync().GetAllEmployeesAsync(pramter));

}
