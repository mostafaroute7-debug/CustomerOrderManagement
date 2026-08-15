
using CustomerOrderManagement.Domain.Common;
using System.Collections.Generic;

namespace CustomerOrderManagement.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new HashSet<CustomerOrder>();
    }
}
