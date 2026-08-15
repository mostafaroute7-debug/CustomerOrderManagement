using CustomerOrderManagement.Domain.Enums;
using System.Collections.Generic;

namespace CustomerOrderManagement.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<int> CustomerIds { get; set; }
    }
}
