using CustomerOrderManagement.Domain.Entities;

namespace CustomerOrderManagement.Application.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenaricRepository<Customer>
    {
        Customer GetByEmail(string email);
        Customer GetByPhone(string phone);
    }
}
