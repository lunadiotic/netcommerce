using Netcom.Api.DTOs;
using Netcom.Api.Models;

namespace Netcom.Api.Services;

public interface IOrderService
{
    Task<Order?> GetOrderDetailsAsync(int orderId);
    Task<Order?> CreateOrderAsync(OrderCreateDto orderDto);
}