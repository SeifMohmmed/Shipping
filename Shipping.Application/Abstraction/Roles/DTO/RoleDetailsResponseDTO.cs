namespace Shipping.Application.Abstraction.Roles.DTO;
public record RoleDetailsResponseDTO
 (string RoleId, string RoleName, string CreatedAt, IEnumerable<string> Permissions);