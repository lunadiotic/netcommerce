namespace Netcom.Api.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property for related orders (if applicable)
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}