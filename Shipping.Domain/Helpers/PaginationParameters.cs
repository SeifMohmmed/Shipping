namespace Shipping.Domain.Helpers;
public class PaginationParameters
{
    private const int MaxPageSize = 10;
    private int? pageSize;

    public int? PageSize
    {
        get { return pageSize; }
        set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
    }
    public int? PageNumber { get; set; } = 1;
}
