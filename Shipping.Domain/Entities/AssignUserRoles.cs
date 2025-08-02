namespace Shipping.Domain.Entities;
public class AssignUserRoles
{
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
