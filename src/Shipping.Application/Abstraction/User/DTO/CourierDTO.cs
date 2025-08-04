namespace Shipping.Application.Abstraction.User.DTO;
public record CourierDTO
{
    public string CourierId { get; set; } = string.Empty;
    public string CourierName { get; set; } = string.Empty;
}