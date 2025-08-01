using Microsoft.AspNetCore.Identity;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class EmployeeRepository(UserManager<ApplicationUser> userManager,
    ApplicationDbContext context) : IEmployeeRepository
{
    public async Task<IEnumerable<ApplicationUser>> GetAllEmployeesAsync(PaginationParameters pramter)
    {
        var merchantIds = (await userManager.GetUsersInRoleAsync(DefaultRole.Merchant)).Select(u => u.Id);
        var courierIds = (await userManager.GetUsersInRoleAsync(DefaultRole.Courier)).Select(u => u.Id);
        var adminIds = (await userManager.GetUsersInRoleAsync(DefaultRole.Admin)).Select(u => u.Id);
        var excludedIds = merchantIds
       .Concat(courierIds)
       .Concat(adminIds)
       .ToHashSet();

        var allEmployees = context.Users
       .Where(u => !excludedIds.Contains(u.Id));

        if (pramter.PageSize != null && pramter.PageNumber != null)
        {
            return allEmployees
                .Skip((pramter.PageNumber.Value - 1) * pramter.PageSize.Value)
                .Take(pramter.PageSize.Value);
        }
        else
        {
            return allEmployees;
        }
    }
}
