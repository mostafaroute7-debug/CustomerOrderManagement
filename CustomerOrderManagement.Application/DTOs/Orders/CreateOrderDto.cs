using CustomerOrderManagement.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerOrderManagement.Application.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required]
        [Range(0.01, 9999999)]
        public decimal TotalAmount { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        public List<int> CustomerIds { get; set; }
    }
}
