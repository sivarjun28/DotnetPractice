namespace ECommerceOrderSystem.Controllers
{
    using ECommerceOrderSystem.Models.Entities;
    using ECommerceOrderSystem.Models.Requests;
    using ECommerceOrderSystem.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("{customerId}")]
    public async Task<IActionResult> CreateOrder(int customerId, [FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(customerId, request);
            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var order = await _orderService.GetOrderWithDetailsAsync(orderId);

        if (order == null)
            return NotFound("Order not found");

        return Ok(order);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetCustomerOrders(int customerId)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId);
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateStatus(int orderId, [FromQuery] OrderStatus status)
    {
        try
        {
            var order = await _orderService.UpdateOrderStatusAsync(orderId, status);
            return Ok(order);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{orderId}/payment")]
    public async Task<IActionResult> ProcessPayment(int orderId, [FromBody] ProcessPaymentRequest request)
    {
        try
        {
            var payment = await _orderService.ProcessPaymentAsync(orderId, request);
            return Ok(payment);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{orderId}/total")]
    public async Task<IActionResult> GetTotal(int orderId)
    {
        var total = await _orderService.CalculateOrderTotalAsync(orderId);
        return Ok(new { Total = total });
    }
}
}