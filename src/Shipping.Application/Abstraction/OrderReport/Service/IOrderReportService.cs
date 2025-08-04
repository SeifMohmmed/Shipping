using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.OrderReport.Service;
public interface IOrderReportService
{
    Task<IEnumerable<OrderReportToShowDTO>> GetAllOrderReportAsync(OrderReportPramter pramter);
    Task<OrderReportDTO> GetOrderReportAsync(int id);
    Task AddAsync(OrderReportDTO DTO);
    Task UpdateAsync(int id, OrderReportDTO DTO);
    Task DeleteAsync(int id);
}
