using Microsoft.AspNetCore.Identity;

namespace Shipping.Domain.Entities;
public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
        Id = Guid.CreateVersion7().ToString();
    }

    public bool IsDeleted { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

}
