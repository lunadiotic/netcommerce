using Microsoft.EntityFrameworkCore;

namespace Netcom.Api.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    [Precision(18, 2)]
    public decimal TotalAmount { get; set; }

    // Navigation property for related order items
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}