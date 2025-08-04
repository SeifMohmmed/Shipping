using Microsoft.AspNetCore.Mvc;
using Shipping.API.Filters;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrdersController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<IEnumerable<OrderWithProductsDTO>>> GetAll([FromQuery] PaginationParameters pramter)
    {
        var orders = await serviceManager.orderService.GetOrdersAsync(pramter);

        return Ok(orders);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<OrderWithProductsDTO>> GetById(int id)
    {
        var order = await serviceManager.orderService.GetOrderAsync(id);

        return Ok(order);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.AddOrders)]
    public async Task<ActionResult<AddOrderDTO>> AddOrder(AddOrderDTO DTO)
    {
        var order = await serviceManager.orderService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO DTO)
    {
        await serviceManager.orderService.UpdateAsync(id, DTO);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HasPermission(Permissions.DeleteOrders)]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        await serviceManager.orderService.DeleteAsync(id);
        return NoContent();
    }


    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<IActionResult> GetAllOrderByStatus([FromQuery] OrderStatus status, [FromQuery] PaginationParameters pramter)
    {
        var orders = await serviceManager.orderService.GetOrdersByStatus(status, pramter);

        return Ok(orders);
    }


    [HttpGet("status/waiting")]
    [HasPermission(Permissions.ViewOrders)]
    public async Task<ActionResult<IEnumerable<OrderWithProductsDTO>>> GetAllWaitingOrders([FromQuery] PaginationParameters pramter)
    {
        var orders =
            await serviceManager.orderService.GetAllWatingOrder(pramter);

        return Ok(orders);
    }


    [HttpPatch("{id}/status/pending")]
    [HasPermission(Permissions.UpdateOrders)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeOrderStatusToPending(int id)
    {
        await serviceManager.orderService.ChangeOrderStatusToPending(id);

        return NoContent();
    }


    [HttpPatch("{id}/status/declined")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<IActionResult> ChangeOrderStatusToDeclined(int id)
    {
        await serviceManager.orderService.ChangeOrderStatusToDeclined(id);
        return NoContent();
    }


    [HttpPatch("{id}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        await serviceManager.orderService.ChangeOrderStatus(id, status);
        return NoContent();
    }


    [HttpPost("{OrderId}/assign/{courierId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HasPermission(Permissions.UpdateOrders)]
    public async Task<IActionResult> AssignOrderToCourier(int OrderId, string courierId)
    {
        await serviceManager.orderService.AssignOrderToCourier(OrderId, courierId);
        return NoContent();
    }
}