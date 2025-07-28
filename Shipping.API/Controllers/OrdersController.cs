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
    public async Task<ActionResult<OrderWithProductsDTO>> Get(int id)
    {
        var order = await _serviceManager.orderService.GetOrderAsync(id);

        if (order is null)
            return NotFound();

        return Ok(order);
    }


    [HttpPost]
    public async Task<ActionResult<AddOrderDTO>> AddOrder(AddOrderDTO DTO)
    {
        if (DTO is null)
            return BadRequest("Invalid Order data");

        await _serviceManager.orderService.AddAsync(DTO);

        return Ok();
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO DTO)
    {
        if (DTO is null || id != DTO.Id)
            return BadRequest("Invalid branch data.");

        try
        {
            await _serviceManager.orderService.UpdateAsync(DTO);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

    }


    [HttpDelete("{id}")]
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
            return BadRequest(ex.Message);
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


    [HttpPost("AssignOrderToCourier/{OrderId}/{courierId}")]
    public async Task<IActionResult> AssignOrderToCourier(int OrderId, string courierId)
    {
        try
        {
            if (string.IsNullOrEmpty(courierId))
            {
                return BadRequest("CouierId is required");
            }

            await _serviceManager.orderService.AssignOrderToCourier(OrderId, courierId);
            return NoContent();
        }

        catch (KeyNotFoundException)
        {
            return NotFound("Order Not Found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }


}
