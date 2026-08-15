using CustomerOrderManagement.Domain.Entities;
using System.Linq;

namespace CustomerOrderManagement.Application.Interfaces.Repositories
{
    public interface IOrderRepository : IGenaricRepository<Order>
    {
        IQueryable<Order> GetByCustomerId(int customerId);
    }
}
