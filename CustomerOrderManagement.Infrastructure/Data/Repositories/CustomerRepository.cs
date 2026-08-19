using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Domain.Entities;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using System.Linq;

namespace CustomerOrderManagement.Infrastructure.Data.Repositories
{
    public class CustomerRepository : GenaricRepository<Customer>,ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context)
       : base(context)
        {
        }
        public Customer GetByEmail(string email)
        {
            return GetAll().FirstOrDefault(x => x.Email == email);
        }

        public Customer GetByPhone(string phone)
        {
            return GetAll().FirstOrDefault(x => x.Phone == phone);
        }
    }
}
