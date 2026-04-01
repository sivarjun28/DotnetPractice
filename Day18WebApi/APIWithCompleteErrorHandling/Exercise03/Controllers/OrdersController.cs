using Exercise03.Exceptions;
using Exercise03.Models;
using Exercise03.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
namespace Exercise03.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(request, ct);
                return Ok(order);
            }
            catch (ValidationException ex)
            {

                return BadRequest(ex.Errors);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id, ct);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
        {
            try
            {
                await _orderService.UpdateStatusAsync(id, request.Status, request.Notes, ct);
                return NoContent();
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            try
            {
                await _orderService.CancelOrderAsync(id, ct);
                return NoContent();
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:int}/process-payment")]
        public async Task<IActionResult> ProcessPayment(int id, CancellationToken ct)
        {
            try
            {
                await _orderService.ProcessPaymentAsync(id, ct);
                return NoContent();
            }
            catch (PaymentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}