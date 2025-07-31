using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.CourierReport.Service;
public interface ICourierReportService
{
    Task<IEnumerable<GetAllCourierOrderCountDTO>> GetAllCourierReportAsync(PaginationParameters pramter);
    Task<CourierReportDTO> GetCourierReportAsync(int id, PaginationParameters parameter);
}
