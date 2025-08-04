using AutoMapper;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.People;
using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Application.Services.EmployeeServices;
internal class EmployeeService(ILogger<EmployeeService> logger,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IEmployeeService
{
    public async Task<IEnumerable<EmployeeDTO>> GetEmployeesAsync(PaginationParameters pramter)
    {
        logger.LogInformation("Getting all employees with pagination {@Pagination}", pramter);

        var employees = await unitOfWork.GetAllEmployeesAsync().GetAllEmployeesAsync(pramter);

        return mapper.Map<IEnumerable<EmployeeDTO>>(employees);
    }
}
