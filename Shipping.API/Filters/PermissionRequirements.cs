using Microsoft.AspNetCore.Authorization;

namespace Shipping.API.Filters;

public class PermissionRequirements(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
