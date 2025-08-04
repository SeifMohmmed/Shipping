using Microsoft.AspNetCore.Identity;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;

namespace Shipping.Infrastructure.Repositories;
internal class MerchantRepository(UserManager<ApplicationUser> userManager) : IMerchantRepository
{
    public async Task<IEnumerable<ApplicationUser>> GetAllMerchantAsync()
    => await userManager.GetUsersInRoleAsync(DefaultRole.Merchant);

}
