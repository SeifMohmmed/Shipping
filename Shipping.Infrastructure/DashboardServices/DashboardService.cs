using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.Dashboard;
using Shipping.Application.Abstraction.Dashboard.DTO;
using Shipping.Domain.Enums;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.DashboardServices;
internal class DashboardService(ILogger<DashboardService> logger,
    ApplicationDbContext context) : IDashboardService
{
    public async Task<MerchantDashboardDTO> GetDashboardDataForMerchantAsync()
    {
        logger.LogInformation("Starting to fetch merchant dashboard data.");

        var statusCounts = await context.Orders.GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        logger.LogInformation("Fetched order counts grouped by status: {@StatusCounts}", statusCounts);

        int GetCount(OrderStatus status) =>
            statusCounts.FirstOrDefault(s => s.Status == status)?.Count ?? 0;


        var dashboardData = new MerchantDashboardDTO
        {
            TotalDelivered = GetCount(OrderStatus.Delivered),
            TotalPending = GetCount(OrderStatus.Pending),
            TotalAwaitingConfirmation = GetCount(OrderStatus.WaitingForConfirmation),
            TotalCancelledByTheRecipient = GetCount(OrderStatus.CanceledByRecipient),
            TotalRejectedWithPayed = GetCount(OrderStatus.DeclinedWithFullPayment),
            TotalPostponed = GetCount(OrderStatus.InProgress),
            TotalDeliveredToTheRepresentative = GetCount(OrderStatus.DeliveredToCourier),
            TotalRejectedWithPartialPayment = GetCount(OrderStatus.DeclinedWithPartialPayment),
            TotalRejectedAndAotPaid = GetCount(OrderStatus.Declined),
            TotalPartiallyDelivered = GetCount(OrderStatus.PartialDelivery),
            TotalCantAccess = GetCount(OrderStatus.UnreachableCustomer)
        };

        return dashboardData;
    }

    public async Task<EmpDashboardDTO> GetDashboardOfEmployeeAsync()
    {
        logger.LogInformation("Starting to fetch employee dashboard data.");

        var statusCounts = await context.Orders
            .GroupBy(o => o.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        logger.LogInformation("Fetched order counts grouped by status: {@StatusCounts}", statusCounts);

        int GetCount(OrderStatus status) =>
            statusCounts.FirstOrDefault(s => s.Status == status)?.Count ?? 0;


        var dashboardData = new EmpDashboardDTO
        {
            TotalDelivered = GetCount(OrderStatus.Delivered),
            TotalPending = GetCount(OrderStatus.Pending),
            TotalAwaitingConfirmation = GetCount(OrderStatus.WaitingForConfirmation),
            TotalInProcessing = GetCount(OrderStatus.InProgress),
            TotalRejected = GetCount(OrderStatus.Declined),
            TotalReturned = GetCount(OrderStatus.CanceledByRecipient),
            TotalCancelled = GetCount(OrderStatus.UnreachableCustomer),
            TotalShipped = GetCount(OrderStatus.DeliveredToCourier),
            TotalReceived = GetCount(OrderStatus.PartialDelivery),
            TotalPayed = GetCount(OrderStatus.DeclinedWithFullPayment),
            TotalUpdated = GetCount(OrderStatus.DeclinedWithPartialPayment)
        };

        return dashboardData;
    }
}
