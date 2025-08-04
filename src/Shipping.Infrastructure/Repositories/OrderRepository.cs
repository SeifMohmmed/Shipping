using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class OrderRepository(ApplicationDbContext context) : GenericRepository<Order, int>(context), IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllWatingOrder(PaginationParameters pramter)
   => await GetOrdersByStatus(OrderStatus.WaitingForConfirmation, pramter);


    public async Task<IEnumerable<Order>> GetOrdersByStatus(OrderStatus status, PaginationParameters pramter)
    {
        var orders = context.Orders.Where(x => x.Status == status);

        if (orders is null)
        {
            return null!;
        }

        if (pramter.PageSize is not null && pramter.PageNumber is not null)
        {
            return await orders
                .Skip((pramter.PageNumber.Value - 1) * pramter.PageSize.Value)
                .Take(pramter.PageSize.Value)
                .ToListAsync();
        }

        else
        {
            return await orders.ToListAsync();
        }
    }
}
