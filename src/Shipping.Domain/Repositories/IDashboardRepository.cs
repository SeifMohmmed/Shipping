using Shipping.Domain.Enums;

namespace Shipping.Domain.Repositories;
public interface IDashboardRepository
{
    Task<Dictionary<OrderStatus, int>> GetOrderStatusCountsAsync();
}
