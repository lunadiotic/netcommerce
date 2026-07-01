using Microsoft.EntityFrameworkCore;
using Netcom.Api.Data;
using Netcom.Api.DTOs;
using Netcom.Api.Models;

namespace Netcom.Api.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetOrderDetailsAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<Order?> CreateOrderAsync(OrderCreateDto orderDto)
    {
        // Validasi User
        var user = await _context.Users.FindAsync(orderDto.UserId);
        if (user == null) return null;

        var order = new Order
        {
            UserId = orderDto.UserId,
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var itemDto in orderDto.Items)
        {
            var product = await _context.Products.FindAsync(itemDto.ProductId);
            if (product == null || product.Stock < itemDto.Quantity)
                throw new Exception($"Produk ID {itemDto.ProductId} tidak valid atau stok tidak cukup.");

            // Kurangi stok produk
            product.Stock -= itemDto.Quantity;

            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };

            totalAmount += (orderItem.Quantity * orderItem.UnitPrice);
            order.OrderItems.Add(orderItem);
        }

        order.TotalAmount = totalAmount;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }
}