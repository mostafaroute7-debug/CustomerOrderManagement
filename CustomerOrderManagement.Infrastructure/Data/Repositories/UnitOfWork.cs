
using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using CustomerOrderManagement.Application.Interfaces;
namespace CustomerOrderManagement.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private ICustomerRepository _customers;
        private IOrderRepository _orders;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customers => _customers == null ? new CustomerRepository(_context) : _customers;
          

        public IOrderRepository Orders => _orders == null ? new OrderRepository(_context) : _orders;
            

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
