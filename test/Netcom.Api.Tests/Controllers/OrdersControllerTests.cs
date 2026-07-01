using Microsoft.AspNetCore.Mvc;
using Moq;
using Netcom.Api.Controllers;
using Netcom.Api.DTOs;
using Netcom.Api.Models;
using Netcom.Api.Services;
using Xunit;

namespace Netcom.Api.Tests.Controllers;

public class OrdersControllerTests
{
    private readonly Mock<IOrderService> _mockService;
    private readonly OrdersController _controller;

    public OrdersControllerTests()
    {
        // Inisialisasi Mock Service
        _mockService = new Mock<IOrderService>();

        // Inject Mock ke Controller
        _controller = new OrdersController(_mockService.Object);
    }

    [Fact]
    public async Task GetOrder_ShouldReturnOkResult_WhenOrderExists()
    {
        // 1. Arrange: Buat skenario data tiruan
        var fakeOrder = new Order { Id = 1, TotalAmount = 500000 };

        // Beritahu Mock: "Jika metode GetOrderDetailsAsync(1) dipanggil, kembalikan fakeOrder"
        _mockService.Setup(s => s.GetOrderDetailsAsync(1)).ReturnsAsync(fakeOrder);

        // 2. Act
        var result = await _controller.GetOrder(1);

        // 3. Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var order = Assert.IsType<Order>(okResult.Value);

        Assert.Equal(1, order.Id);
    }

    [Fact]
    public async Task GetOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        // 1. Arrange: Beritahu mock untuk mengembalikan null
        _mockService.Setup(s => s.GetOrderDetailsAsync(99)).ReturnsAsync((Order?)null);

        // 2. Act
        var result = await _controller.GetOrder(99);

        // 3. Assert
        var actionResult = Assert.IsType<ActionResult<Order>>(result);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    }
}