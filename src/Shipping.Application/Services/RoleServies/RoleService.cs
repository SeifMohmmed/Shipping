using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shipping.Application.Abstraction.Roles;
using Shipping.Application.Abstraction.Roles.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;
using System.Data;

namespace Shipping.Infrastructure.RoleServices;
public class RoleService(ILogger<RoleService> logger,
    RoleManager<ApplicationRole> roleManager,
    IRoleRepository roleRepository) : IRoleService
{
    // Get all Roles (Group)
    public async Task<IEnumerable<RoleResponseDTO>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching all active roles");

        var roles = await roleRepository.GetAllActiveRolesAsync(cancellationToken);

        return roles.Select(r => new RoleResponseDTO(
        r.Id,
        r.Name!,
        r.CreatedAt.ToShortDateString()
        ));

    }

    // Get Role (Group) By Id
    public async Task<RoleDetailsResponseDTO?> GetRoleByIdAsync(string roleId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching role details for roleId: {RoleId}", roleId);

        var role = await roleRepository.GetRoleByIdAsync(roleId, cancellationToken);

        if (role is null)
            return null;

        var permissions = await roleRepository.GetRolePermissionsAsync(roleId, cancellationToken);

        return new RoleDetailsResponseDTO(
            role.Id,
            role.Name!,
            role.CreatedAt.ToShortDateString(),
            permissions
            );
    }

    // Create Role (Group)
    public async Task<string> CreateRoleAsync(CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Creating new role: {RoleName}", createRoleRequestDTO.RoleName);

        var isRoleExists = await roleRepository.RoleExistsAsync(createRoleRequestDTO.RoleName, cancellationToken);

        if (isRoleExists)
            return "Role Already Exist !";


        var allowedPermissions = Permissions.GetAllPermissions();

        if (createRoleRequestDTO.Permissions.Except(allowedPermissions).Any())
            return "Invalid Permissions !";

        var role = new ApplicationRole
        {
            Name = createRoleRequestDTO.RoleName,
            ConcurrencyStamp = Guid.CreateVersion7().ToString(),
        };

        var roleCreated = await roleRepository.CreateRoleAsync(role, cancellationToken);

        if (!roleCreated)
            return "Failed To Create Role !";

        var permissionsAdded = await roleRepository.AddPermissionsToRoleAsync(role.Id, createRoleRequestDTO.Permissions, cancellationToken);


        return "Role Added Successfully!";
    }

    // Update Role (Group)
    public async Task<string> UpdateRoleAsync(string roleId, CreateRoleRequestDTO createRoleRequestDTO, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating role: {RoleId} with name: {RoleName}", roleId, createRoleRequestDTO.RoleName);

        var isRoleExist = await roleRepository.RoleExistsExceptCurrentAsync(createRoleRequestDTO.RoleName, roleId, cancellationToken);

        if (isRoleExist)
            return "Role Already Exist !";


        var role = await roleRepository.GetRoleByIdAsync(roleId, cancellationToken);

        if (role is null)
            return "Role does not exists";

        var allowedPermissions = Permissions.GetAllPermissions();

        if (createRoleRequestDTO.Permissions.Except(allowedPermissions).Any())
            return "Invalid Permissions !";

        role.Name = createRoleRequestDTO.RoleName;

        var roleUpdated = await roleRepository.UpdateRoleAsync(role, cancellationToken);

        if (!roleUpdated)
            return "Failed To Delete Role !";

        var permissionsUpdated = await roleRepository.UpdateRolePermissionsAsync(roleId, createRoleRequestDTO.Permissions, cancellationToken);

        if (!permissionsUpdated)
            return "Failed To Update Role !";


        return "Role Updated Successfully!";
    }

    // Delete Role (Group)
    public async Task<string> DeleteRoleAsync(string roleId, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Deleting role: {RoleId}", roleId);

        var role = await roleRepository.GetRoleByIdAsync(roleId, cancellationToken);

        if (role is null)
            return "Role does not Exists !";

        var deleted = await roleRepository.DeleteRoleAsync(roleId, cancellationToken);

        if (!deleted)
            return "Failed To Delete Role !";


        return "Role Deleted Successfully!";
    }
}
