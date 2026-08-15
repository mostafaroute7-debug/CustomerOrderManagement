
using CustomerOrderManagement.Domain.Common;
using CustomerOrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;

namespace CustomerOrderManagement.Domain.Entities
{
    public class Order : BaseEntity
    {

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new HashSet<CustomerOrder>();
    }
}
