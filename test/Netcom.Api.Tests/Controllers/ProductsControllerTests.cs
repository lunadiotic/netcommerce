using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Netcom.Api.Controllers;
using Netcom.Api.Data;
using Netcom.Api.Models;
using Netcom.Api.Services;
using Xunit;

namespace Netcom.Api.Tests;

public class ProductsControllerTests
{
    // Helper method untuk membuat DbContext In-Memory yang unik untuk setiap test
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nama DB unik agar test tidak saling bertabrakan
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnAllProducts_WhenProductsExist()
    {
        // 1. Arrange
        using var context = GetInMemoryDbContext();
        context.Products.AddRange(
            new Product { Name = "Laptop", Price = 15000000, Stock = 10 },
            new Product { Name = "Mouse", Price = 200000, Stock = 50 }
        );
        await context.SaveChangesAsync();

        var productService = new ProductService(context);
        var controller = new ProductsController(productService);

        // 2. Act
        var result = await controller.GetProducts();

        // 3. Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

        Assert.Equal(2, products.Count());
    }

    [Fact]
    public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // 1. Arrange
        using var context = GetInMemoryDbContext();
        var productService = new ProductService(context);
        var controller = new ProductsController(productService);

        // 2. Act
        var result = await controller.GetProduct(999); // ID yang tidak ada

        // 3. Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostProduct_ShouldCreateProduct_AndReturnCreatedAtAction()
    {
        // 1. Arrange
        using var context = GetInMemoryDbContext();
        var productService = new ProductService(context);
        var controller = new ProductsController(productService);
        var newProduct = new Product { Name = "Keyboard Mechanical", Price = 800000, Stock = 15 };

        // 2. Act
        var result = await controller.CreateProduct(newProduct);

        // 3. Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdProduct = Assert.IsType<Product>(actionResult.Value);

        Assert.Equal("Keyboard Mechanical", createdProduct.Name);
        Assert.True(createdProduct.Id > 0); // Memastikan ID digenerate otomatis
        Assert.Equal(1, await context.Products.CountAsync()); // Memastikan benar-benar masuk ke DB
    }
}