using Microsoft.AspNetCore.Mvc;
using Exercise02.Models;
using Exercise02.ModelBinders;

namespace Exercise02.Controller
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private static readonly List<Order> Orders = new();

        // -----------------------------
        // Task 1: Get order by ID (from route)
        // -----------------------------
        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // -----------------------------
        // Task 2: Search orders (complex query parameters)
        // -----------------------------
        [HttpGet("search")]
        public IActionResult Search([FromQuery] OrderSearchParams searchParams)
        {
            var query = Orders.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.CustomerId))
                query = query.Where(o => o.CustomerId == searchParams.CustomerId);

            if (!string.IsNullOrEmpty(searchParams.Status))
                query = query.Where(o => o.Status == searchParams.Status);

            if (searchParams.MinTotal.HasValue)
                query = query.Where(o => o.Total >= searchParams.MinTotal.Value);

            if (searchParams.MaxTotal.HasValue)
                query = query.Where(o => o.Total <= searchParams.MaxTotal.Value);

            if (searchParams.FromDate.HasValue)
                query = query.Where(o => o.Date >= searchParams.FromDate.Value);

            if (searchParams.ToDate.HasValue)
                query = query.Where(o => o.Date <= searchParams.ToDate.Value);

            query = searchParams.SortBy.ToLower() switch
            {
                "total" => searchParams.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(o => o.Total)
                    : query.OrderBy(o => o.Total),
                _ => searchParams.SortDirection.ToLower() == "desc"
                    ? query.OrderByDescending(o => o.Date)
                    : query.OrderBy(o => o.Date)
            };

            var result = query
                .Skip((searchParams.Page - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize)
                .ToList();

            return Ok(result);
        }

        // -----------------------------
        // Task 3: Create order (from body)
        // -----------------------------
        [HttpPost]
        public IActionResult Create([FromBody] CreateOrderRequest request)
        {
            var newOrder = new Order
            {
                Id = Orders.Count + 1,
                CustomerId = request.CustomerId,
                Items = request.Items,
                ShippingAddress = request.ShippingAddress,
                PaymentInfo = request.PaymentInfo,
                Total = request.Items.Sum(i => i.Price * i.Quantity),
                Status = "Pending",
                Date = DateTime.UtcNow
            };
            Orders.Add(newOrder);
            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }

        // -----------------------------
        // Task 4: Update order with header + body
        // -----------------------------
        [HttpPut("{id:int}")]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] UpdateOrderRequest request,
            [FromHeader(Name = "If-Match")] string? etag)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            // Simple ETag validation example
            var currentEtag = order.Date.Ticks.ToString();
            if (etag != null && etag != currentEtag)
                return StatusCode(412, "ETag mismatch");

            order.Status = request.Status;
            if (request.NewShippingAddress != null)
                order.ShippingAddress = request.NewShippingAddress;

            return Ok(order);
        }

        // -----------------------------
        // Task 5: Bulk create with API key
        // -----------------------------
        [HttpPost("bulk")]
        public IActionResult BulkCreate(
            [FromBody] List<CreateOrderRequest> ordersRequests,
            [FromHeader(Name = "X-API-Key")] string apiKey,
            [FromHeader(Name = "X-Idempotency-Key")] string? idempotencyKey)
        {
            if (apiKey != "SECRET123") return Unauthorized("Invalid API key");

            var createdOrders = new List<Order>();
            foreach (var request in ordersRequests)
            {
                var newOrder = new Order
                {
                    Id = Orders.Count + 1,
                    CustomerId = request.CustomerId,
                    Items = request.Items,
                    ShippingAddress = request.ShippingAddress,
                    PaymentInfo = request.PaymentInfo,
                    Total = request.Items.Sum(i => i.Price * i.Quantity),
                    Status = "Pending",
                    Date = DateTime.UtcNow
                };
                Orders.Add(newOrder);
                createdOrders.Add(newOrder);
            }
            return Ok(createdOrders);
        }

        // -----------------------------
        // Task 6: Upload order documents
        // -----------------------------
        [HttpPost("{id:int}/documents")]
        public async Task<IActionResult> UploadDocuments(
            [FromRoute] int id,
            [FromForm] OrderDocumentUpload upload)
        {
            var order = Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();

            // For demo, just read file names
            var filesInfo = upload.Files.Select(f => f.FileName).ToList();
            return Ok(new { OrderId = id, upload.DocumentType, Files = filesInfo });
        }

        // -----------------------------
        // Task 7: Get multiple orders by comma-separated IDs
        // -----------------------------
        [HttpGet("batch")]
        public IActionResult GetMultiple(
    [FromQuery][ModelBinder(typeof(CommaSeparatedModelBinder))] int[] ids)
        {
            var orders = Orders.Where(o => ids.Contains(o.Id)).ToList();
            return Ok(orders);
        }
    }
}