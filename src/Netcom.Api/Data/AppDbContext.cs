using Netcom.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Netcom.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}