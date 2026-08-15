
using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Domain.Entities;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using System.Linq;

namespace CustomerOrderManagement.Infrastructure.Data.Repositories
{
    public class OrderRepository : GenaricRepository<Order>,IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context)
       : base(context)
        {
        }

        public IQueryable<Order> GetByCustomerId(int customerId)
        {
            return GetAll()
                .Where(x =>
                    x.CustomerOrders
                     .Any(co => co.CustomerId == customerId));
        }
    }
}
