using Microsoft.AspNetCore.Mvc;
using Netcom.Api.DTOs;
using Netcom.Api.Models;
using Netcom.Api.Services;

namespace Netcom.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _orderService.GetOrderDetailsAsync(id);

        if (order == null) return NotFound("Pesanan tidak ditemukan.");

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderCreateDto orderDto)
    {
        try
        {
            var createdOrder = await _orderService.CreateOrderAsync(orderDto);

            if (createdOrder == null) return BadRequest("User tidak ditemukan.");

            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message); // Mengembalikan pesan error stok tidak cukup
        }
    }
}