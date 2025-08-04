using Shipping.Domain.Enums;

namespace Shipping.Domain.Helpers;
public class OrderReportPramter
{
    private const int MaxPageSize = 10;
    private int? pageSize;
    public int? PageSize
    {
        get { return pageSize; }
        set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
    }
    public int? PageNumber { get; set; } = 1;

    public DateTime? DateFrom { get; set; } = DateTime.Now.AddDays(-30);

    public DateTime? DateTo { get; set; } = DateTime.Now;

    public OrderStatus? OrderStatus { get; set; }

}
