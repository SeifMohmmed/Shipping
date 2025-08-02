namespace Shipping.Domain.Entities;
public class UnassignUserRoles
{
    public string UserEmail { get; set; } = default!;

    public string RoleName { get; set; } = default!;
}
