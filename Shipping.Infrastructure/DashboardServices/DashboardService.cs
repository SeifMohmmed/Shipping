using Shipping.Application.Abstraction.Dashboard;
using Shipping.Application.Abstraction.Dashboard.DTO;
using Shipping.Domain.Enums;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.DashboardServices;
internal class DashboardService(ApplicationDbContext _Context) : IDashboardService
{
    public async Task<MerchantDashboardDTO> GetDashboardDataForMerchantAsync()
    {
        int TotalDelivered = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.Delivered));
        int TotalPending = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.Pending));
        int TotalAwaitingConfirmation = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.WaitingForConfirmation));
        int TotalCancelledByTheRecipient = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.CanceledByRecipient));
        int TotalRejectedWithPayed = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.DeclinedWithFullPayment));
        int TotalPostponed = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.InProgress));
        int TotalDeliveredToTheRepresentative = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.DeliveredToCourier));
        int TotalRejectedWithPartialPayment = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.DeclinedWithPartialPayment));
        int TotalRejectedAndAotPaid = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.Declined));
        int TotalPartiallyDelivered = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.PartialDelivery));
        int TotalCantAccess = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.UnreachableCustomer));

        var dashboardData = new MerchantDashboardDTO
        {
            TotalDelivered = TotalDelivered,
            TotalPending = TotalPending,
            TotalCancelledByTheRecipient = TotalCancelledByTheRecipient,
            TotalAwaitingConfirmation = TotalAwaitingConfirmation,
            TotalRejectedWithPayed = TotalRejectedWithPayed,
            TotalPostponed = TotalPostponed,
            TotalDeliveredToTheRepresentative = TotalDeliveredToTheRepresentative,
            TotalRejectedWithPartialPayment = TotalRejectedWithPartialPayment,
            TotalRejectedAndAotPaid = TotalRejectedAndAotPaid,
            TotalPartiallyDelivered = TotalPartiallyDelivered,
            TotalCantAccess = TotalCantAccess
        };

        return dashboardData;
    }

    public async Task<EmpDashboardDTO> GetDashboardOfEmployeeAsync()
    {
        int TotalDelivered = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.Delivered));
        int TotalPending = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.Pending));
        int TotalAwaitingConfirmation = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.WaitingForConfirmation));
        int TotalInProcessing = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.InProgress));
        int TotalRejected = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.Declined));
        int TotalReturned = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.CanceledByRecipient));
        int TotalCancelled = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.UnreachableCustomer));
        int TotalShipped = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.DeliveredToCourier));
        int TotalReceived = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.PartialDelivery));
        int TotalPayed = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.DeclinedWithFullPayment));
        int TotalUpdated = await Task.Run(() => _Context.Orders.Count(x => x.Status == OrderStatus.DeclinedWithPartialPayment));

        var dashboardData = new EmpDashboardDTO
        {
            TotalDelivered = TotalDelivered,
            TotalPending = TotalPending,
            TotalCancelled = TotalCancelled,
            TotalInProcessing = TotalInProcessing,
            TotalAwaitingConfirmation = TotalAwaitingConfirmation,
            TotalRejected = TotalRejected,
            TotalReceived = TotalReceived,
            TotalShipped = TotalShipped,
            TotalReturned = TotalReturned,
            TotalPayed = TotalPayed,
            TotalUpdated = TotalUpdated
        };

        return dashboardData;
    }
}
