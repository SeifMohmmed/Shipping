using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Domain.Enums;
using Shipping.Domain.Helpers;

namespace Shipping.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrdersController(IServiceManager _serviceManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderWithProductsDTO>>> GetAll([FromQuery] PaginationParameters pramter)
    {
        var orders = await _serviceManager.orderService.GetOrdersAsync(pramter);

        return Ok(orders);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderWithProductsDTO>> GetById(int id)
    {
        var order = await _serviceManager.orderService.GetOrderAsync(id);

        if (order is null)
            return NotFound($"Order with id {id} was not found.");

        return Ok(order);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AddOrderDTO>> AddOrder(AddOrderDTO DTO)
    {
        var order = await _serviceManager.orderService.AddAsync(DTO);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO DTO)
    {
        try
        {
            await _serviceManager.orderService.UpdateAsync(id, DTO);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        try
        {
            await _serviceManager.orderService.DeleteAsync(id);
            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet("GetAllOrderByStatus")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrderByStatus(OrderStatus status, [FromQuery] PaginationParameters pramter)
    {
        try
        {
            var orders = await _serviceManager.orderService.GetOrdersByStatus(status, pramter);

            if (orders.Count() == 0)
                return NotFound("No Orders Found !");

            return Ok(orders);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpGet("GetAllWaitingOrders")]
    public async Task<ActionResult<IEnumerable<OrderWithProductsDTO>>> GetAllWaitingOrder([FromQuery] PaginationParameters pramter)
    {
        var orders =
            await _serviceManager.orderService.GetAllWatingOrder(pramter);

        return Ok(orders);
    }


    [HttpPost("ChangeOrderStatusToPending/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeOrderStatusToPending(int id)
    {
        try
        {
            await _serviceManager.orderService.ChangeOrderStatusToPending(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPost("ChangeOrderStatusToDeclined/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeOrderStatusToDeclined(int id)
    {
        try
        {
            await _serviceManager.orderService.ChangeOrderStatusToDeclined(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPost("UpdateStatus/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
    {
        try
        {
            await _serviceManager.orderService.ChangeOrderStatus(id, status);
            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }


    [HttpPost("{OrderId}/AssignOrderToCourier/{courierId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignOrderToCourier(int OrderId, string courierId)
    {
        if (string.IsNullOrEmpty(courierId))
            return NotFound($"courierId with id {courierId} was not found.");

        try
        {
            await _serviceManager.orderService.AssignOrderToCourier(OrderId, courierId);
            return NoContent();
        }

        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
