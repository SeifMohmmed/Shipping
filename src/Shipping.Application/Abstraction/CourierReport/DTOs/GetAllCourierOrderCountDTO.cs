namespace Shipping.Application.Abstraction.CourierReport.DTOs;
public class GetAllCourierOrderCountDTO
{
    public required string CourierName { get; set; }

    public int OrdersCount { get; set; } = 0;

}
