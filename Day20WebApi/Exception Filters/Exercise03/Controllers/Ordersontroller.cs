using Exercise03.Filters;
using Exercise03.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exercise03.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [ServiceFilter(typeof(ValidationExceptionFilter))]
    [ServiceFilter(typeof(NotFoundExceptionFilter))]
    [ServiceFilter(typeof(BusinessRuleExceptionFilter))]
    [ServiceFilter(typeof(GlobalExceptionFilter))] // last
    public class OrdersController : ControllerBase
    {
        private static readonly List<Order> Orders = new();

        [HttpPost]
        public ActionResult<Order> Create(OrderDto dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                throw new ValidationException(new Dictionary<string, string[]>
                {
                    { "Items", new[] { "Order must contain at least one item" } }
                });

            if (dto.TotalAmount < 10)
                throw new BusinessRuleException(
                    "MIN_ORDER_AMOUNT",
                    "Order total must be at least $10",
                    new Dictionary<string, object>
                    {
                        { "MinAmount", 10 },
                        { "CurrentAmount", dto.TotalAmount }
                    });

            var newOrder = new Order
            {
                CustomerName = dto.CustomerName,
                Items = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList(),
                TotalAmount = dto.TotalAmount
            };

            Orders.Add(newOrder);
            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetById(string id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
                throw new NotFoundException("Order", id);

            return Ok(order);
        }
    }
}