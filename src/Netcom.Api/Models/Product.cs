using Microsoft.EntityFrameworkCore;
namespace Netcom.Api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}