using Shipping.Application.Abstraction.Dashboard.DTO;

namespace Shipping.Application.Abstraction.Dashboard;
public interface IDashboardService
{
    Task<EmpDashboardDTO> GetDashboardOfEmployeeAsync();
    Task<MerchantDashboardDTO> GetDashboardDataForMerchantAsync();
}
