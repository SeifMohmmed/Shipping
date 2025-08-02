using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Merchant.DTO;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MerchantController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewMerchants)]
    public async Task<ActionResult<IEnumerable<MerchantDTO>>> GetAllMerchant()
    {
        var merchants = await serviceManager.merchantService.GetMerchantAsync();

        return Ok(merchants);
    }
}
