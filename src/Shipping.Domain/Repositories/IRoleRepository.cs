using Shipping.Domain.Entities;

namespace Shipping.Domain.Repositories;
public interface IRoleRepository
{
    Task<IEnumerable<ApplicationRole>> GetAllActiveRolesAsync(CancellationToken cancellationToken = default);
    Task<ApplicationRole?> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default);
    Task<ApplicationRole?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken = default);
    Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken = default);
    Task<bool> RoleExistsExceptCurrentAsync(string roleName, string excludeRoleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetRolePermissionsAsync(string roleId, CancellationToken cancellationToken = default);
    Task<bool> CreateRoleAsync(ApplicationRole role, CancellationToken cancellationToken = default);
    Task<bool> UpdateRoleAsync(ApplicationRole role, CancellationToken cancellationToken = default);
    Task<bool> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default);
    Task<bool> AddPermissionsToRoleAsync(string roleId, IEnumerable<string> permissions, CancellationToken cancellationToken = default);
    Task<bool> UpdateRolePermissionsAsync(string roleId, IEnumerable<string> newPermissions, CancellationToken cancellationToken = default);
}