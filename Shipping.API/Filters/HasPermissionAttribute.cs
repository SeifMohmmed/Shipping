using Microsoft.AspNetCore.Authorization;

namespace Shipping.API.Filters;

public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{

}
