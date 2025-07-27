using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Domain.Pramter_Helper;

namespace Shipping.Application.Abstraction.CourierReport.Service;
public interface ICourierReportService
{
    Task<IEnumerable<GetAllCourierOrderCountDTO>> GetAllCourierReportAsync(Pramter pramter);
    Task<CourierReportDTO> GetCourierReportAsync(int id);
}
