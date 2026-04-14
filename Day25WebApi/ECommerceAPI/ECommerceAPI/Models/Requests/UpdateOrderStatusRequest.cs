using ECommerceAPI.Models.Entities;

namespace ECommerceAPI.Models.Requests
{
    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
    }
}