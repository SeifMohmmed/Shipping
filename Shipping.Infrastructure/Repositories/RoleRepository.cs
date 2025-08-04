using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Repositories;
internal class RoleRepository(RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context,
    ILogger<RoleRepository> logger) : IRoleRepository
{
    public async Task<IEnumerable<ApplicationRole>> GetAllActiveRolesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await roleManager.Roles
                .Where(r => !r.IsDeleted)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching all active roles");
            throw;
        }
    }

    public async Task<ApplicationRole?> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await roleManager.FindByIdAsync(roleId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching role by id: {RoleId}", roleId);
            throw;
        }
    }

    public async Task<ApplicationRole?> GetRoleByNameAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            return await roleManager.FindByNameAsync(roleName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching role by name: {RoleName}", roleName);
            throw;
        }
    }

    public async Task<bool> RoleExistsAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            return await roleManager.RoleExistsAsync(roleName);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while checking if role exists: {RoleName}", roleName);
            throw;
        }
    }

    public async Task<bool> RoleExistsExceptCurrentAsync(string roleName, string excludeRoleId, CancellationToken cancellationToken = default)
    {
        try
        {
            return await roleManager.Roles
                .AnyAsync(r => r.Name == roleName && r.Id != excludeRoleId, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while checking if role exists except current: {RoleName}, {ExcludeId}", roleName, excludeRoleId);
            throw;
        }
    }

    public async Task<IEnumerable<string>> GetRolePermissionsAsync(string roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null) return Enumerable.Empty<string>();

            var permissions = await roleManager.GetClaimsAsync(role);
            return permissions.Select(p => p.Value);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching role permissions: {RoleId}", roleId);
            throw;
        }
    }

    public async Task<bool> CreateRoleAsync(ApplicationRole role, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await roleManager.CreateAsync(role);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating role: {RoleName}", role.Name);
            throw;
        }
    }

    public async Task<bool> UpdateRoleAsync(ApplicationRole role, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating role: {RoleId}", role.Id);
            throw;
        }
    }

    public async Task<bool> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role is null) return false;

            role.IsDeleted = true;
            var result = await roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting role: {RoleId}", roleId);
            throw;
        }
    }

    public async Task<bool> AddPermissionsToRoleAsync(string roleId, IEnumerable<string> permissions, CancellationToken cancellationToken = default)
    {
        try
        {
            var roleClaims = permissions.Select(p => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = p,
                RoleId = roleId
            });

            await context.RoleClaims.AddRangeAsync(roleClaims, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while adding permissions to role: {RoleId}", roleId);
            throw;
        }
    }

    public async Task<bool> UpdateRolePermissionsAsync(string roleId, IEnumerable<string> newPermissions, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get current permissions
            var currentPermissions = await context.RoleClaims
                .Where(c => c.RoleId == roleId && c.ClaimType == Permissions.Type)
                .Select(c => c.ClaimValue!)
                .ToListAsync(cancellationToken);

            // Calculate permissions to add and remove
            var permissionsToAdd = newPermissions.Except(currentPermissions);
            var permissionsToRemove = currentPermissions.Except(newPermissions);

            // Remove old permissions
            if (permissionsToRemove.Any())
            {
                await context.RoleClaims
                    .Where(rc => rc.RoleId == roleId && permissionsToRemove.Contains(rc.ClaimValue))
                    .ExecuteDeleteAsync(cancellationToken);
            }

            // Add new permissions
            if (permissionsToAdd.Any())
            {
                var newRoleClaims = permissionsToAdd.Select(p => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = p,
                    RoleId = roleId
                });

                await context.RoleClaims.AddRangeAsync(newRoleClaims, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating role permissions: {RoleId}", roleId);
            throw;
        }
    }
}
