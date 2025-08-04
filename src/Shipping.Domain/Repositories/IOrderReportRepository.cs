using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;

namespace Shipping.Domain.Repositories;
public interface IOrderReportRepository : IGenericRepository<OrderReport, int>
{
    // This Is A Method That Get All OrderReport By Pramter (Pagination , Status)
    Task<IEnumerable<OrderReport>> GetOrderReportByPramter(OrderReportPramter pramter, IQueryable<OrderReport>? include = null);
}
