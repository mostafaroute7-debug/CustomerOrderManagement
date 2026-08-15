
using CustomerOrderManagement.Application.Interfaces.Repositories;

namespace CustomerOrderManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        ICustomerRepository Customers { get; }

        IOrderRepository Orders { get; }

        int SaveChanges();
    }
}
