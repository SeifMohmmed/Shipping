using Shipping.Application.Abstraction.Courier.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.Application.Abstraction.Courier;
public interface ICourierService
{
    Task<IEnumerable<CourierDTO>> GetCourierByBranch(int OrderId);
    Task<IEnumerable<CourierDTO>> GetCourierByRegion(int OrderId, PaginationParameters pramter);
    Task<IEnumerable<CourierDTO>> GetAllAsync(PaginationParameters pramter);
}