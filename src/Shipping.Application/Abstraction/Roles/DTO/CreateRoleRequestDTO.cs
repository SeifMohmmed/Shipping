namespace Shipping.Application.Abstraction.Roles.DTO;
public record CreateRoleRequestDTO
(string RoleName, IEnumerable<string> Permissions);