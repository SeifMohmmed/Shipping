using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Domain.Enums;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
public class DashboardRepository(ApplicationDbContext context,
    ILogger<IDashboardRepository> logger) : IDashboardRepository
{
    public async Task<Dictionary<OrderStatus, int>> GetOrderStatusCountsAsync()
    {
        try
        {
            var statusCounts = await context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return statusCounts.ToDictionary(x => x.Status, x => x.Count);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching order status counts");
            throw;
        }
    }
}
