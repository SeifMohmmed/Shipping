using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstraction.Auth;
using Shipping.Application.Abstraction.Roles.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Infrastructure.Persistence;
using System.Data;

namespace Shipping.Infrastructure.RoleServices;
public class RoleService(RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context) : IRoleService
{
    // Get all Roles (Group)
    public async Task<IEnumerable<RoleResponseDTO>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        return await roleManager.Roles
            .Where(r => !r.IsDeleted)
            .Select(r => new RoleResponseDTO(
                r.Id,
                r.Name!,
                r.CreatedAt.ToShortDateString()
                )).ToListAsync(cancellationToken);
    }

    // Get Role (Group) By Id
    public async Task<RoleDetailsResponseDTO?> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
    {
        if (await roleManager.FindByIdAsync(roleId) is not { } role)
            return null;

        var permissions = await roleManager.GetClaimsAsync(role);

        return new RoleDetailsResponseDTO(
            role.Id,
            role.Name!,
            role.CreatedAt.ToShortDateString(),
            permissions.Select(p => p.Value)
            );
    }

    // Create Role (Group)
    public async Task<string> CreateRoleAsync(CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken = default)
    {
        var IsRoleExists =
            await roleManager.RoleExistsAsync(createRoleRequestDTO.RoleName);

        if (IsRoleExists)
            return "Role Already Exist !";

        var allowedPermissions = Permissions.GetAllPermissions();

        if (createRoleRequestDTO.Permissions.Except(allowedPermissions).Any())
            return "Invalid Permissions !";

        var role = new ApplicationRole
        {
            Name = createRoleRequestDTO.RoleName,
            ConcurrencyStamp = Guid.CreateVersion7().ToString(),
        };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return "Failed To Create Role !";

        var permissions = createRoleRequestDTO.Permissions.Select(p => new IdentityRoleClaim<string>
        {
            ClaimType = Permissions.Type,
            ClaimValue = p,
            RoleId = role.Id
        });

        await context.RoleClaims.AddRangeAsync(permissions, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return "Group Created Successfully!";
    }

    // Update Role (Group)
    public async Task<string> UpdateRoleAsync(string roleId, CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken = default)
    {
        var IsRoleExist =
            await roleManager.Roles.AnyAsync(r => r.Name == createRoleRequestDTO.RoleName && r.Id != roleId);

        if (IsRoleExist)
            return "Role Already Exist !";

        if (await roleManager.FindByIdAsync(roleId) is not { } role)
            return "Role does not exists";

        var allowedPermissions = Permissions.GetAllPermissions();

        if (createRoleRequestDTO.Permissions.Except(allowedPermissions).Any())
            return "Invalid Permissions !";

        role.Name = createRoleRequestDTO.RoleName;

        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            return "Failed To Delete Role !";

        var permissions = await context.RoleClaims
            .Where(c => c.RoleId == roleId && c.ClaimType == Permissions.Type)
            .Select(c => c.ClaimValue)
            .ToListAsync();

        var permissionToAdd = createRoleRequestDTO.Permissions.Except(permissions).Select(p => new IdentityRoleClaim<string>
        {
            ClaimType = Permissions.Type,
            ClaimValue = p,
            RoleId = role.Id
        });

        var permissionsToRemove = permissions.Except(createRoleRequestDTO.Permissions);

        await context.RoleClaims
            .Where(rc => rc.RoleId == roleId && permissionsToRemove.Contains(rc.ClaimValue))
            .ExecuteDeleteAsync(cancellationToken);

        await context.RoleClaims.AddRangeAsync(permissionToAdd, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return "Group Updated Successfully!";
    }

    // Delete Role (Group)
    public async Task<string> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        if (await roleManager.FindByIdAsync(roleId) is not { } role)
            return "Role does not Exists !";

        role.IsDeleted = true;

        var result = await roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            return "Failed To Delete Role !";

        return "Group Deleted Successfully!";
    }
}
