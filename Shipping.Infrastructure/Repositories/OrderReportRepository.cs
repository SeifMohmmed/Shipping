using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class OrderReportRepository(ApplicationDbContext context) : GenericRepository<OrderReport, int>(context), IOrderReportRepository
{
    public async Task<IEnumerable<OrderReport>> GetOrderReportByPramter(OrderReportPramter pramter, IQueryable<OrderReport>? include = null)
    {
        IQueryable<OrderReport> orderReportes = context.OrderReports
            .Include(r => r.Order)
                .ThenInclude(o => o.Region)
            .Include(r => r.Order)
                .ThenInclude(o => o.CitySetting);


        if (pramter.OrderStatus is null && pramter.PageSize is null && pramter.PageNumber is null &&
             pramter.DateFrom is null && pramter.DateTo is null)
        {
            return await orderReportes.ToListAsync();
        }

        if (pramter.OrderStatus is not null)
        {
            orderReportes = orderReportes.Where(x => x.Order != null && x.Order.Status == pramter.OrderStatus);

        }

        if (pramter.DateFrom is not null)
        {
            orderReportes = orderReportes.Where(x => x.ReportDate >= pramter.DateFrom);
        }

        if (pramter.DateTo is not null)
        {
            orderReportes = orderReportes.Where(x => x.ReportDate <= pramter.DateTo);
        }

        if (pramter.PageSize is not null && pramter.PageNumber is not null)
        {
            orderReportes = orderReportes
                .Skip((pramter.PageNumber.Value - 1) * pramter.PageSize.Value)
                .Take(pramter.PageSize.Value);
        }

        return await orderReportes.ToListAsync();

    }
}
