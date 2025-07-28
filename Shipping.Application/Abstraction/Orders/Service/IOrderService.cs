using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.Orders.Service;
public interface IOrderService
{
    Task<IEnumerable<OrderWithProductsDTO>> GetOrdersAsync(PaginationParameters pramter);
    Task<OrderWithProductsDTO> GetOrderAsync(int id);
    Task<IEnumerable<OrderWithProductsDTO>> GetOrdersByStatus(OrderStatus status, PaginationParameters pramter);
    Task<IEnumerable<OrderWithProductsDTO>> GetAllWatingOrder(PaginationParameters pramter);
    Task ChangeOrderStatus(int id, OrderStatus orderStatus);
    Task ChangeOrderStatusToPending(int id);
    Task ChangeOrderStatusToDeclined(int id);
    Task AssignOrderToCourier(int OrderId, string courierId);
    Task AddAsync(AddOrderDTO DTO);
    Task UpdateAsync(UpdateOrderDTO DTO);
    Task DeleteAsync(int id);
}
