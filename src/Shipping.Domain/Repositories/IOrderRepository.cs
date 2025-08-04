using Shipping.Domain.Entities;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;

namespace Shipping.Domain.Repositories;
public interface IOrderRepository : IGenericRepository<Order, int>
{
    // This Is A Method That Get All Orders By pramter (Pagination , Order-Status)
    Task<IEnumerable<Order>> GetOrdersByStatus(OrderStatus status, PaginationParameters pramter);
    Task<IEnumerable<Order>> GetAllWatingOrder(PaginationParameters pramter);
}
