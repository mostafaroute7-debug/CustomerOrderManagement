
using CustomerOrderManagement.Application.Interfaces;
using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Domain.Common;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
namespace CustomerOrderManagement.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        private ICustomerRepository _customers;
        private IOrderRepository _orders;

        public UnitOfWork(ApplicationDbContext context,ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public ICustomerRepository Customers => _customers == null ? new CustomerRepository(_context) : _customers;
        public IOrderRepository Orders => _orders == null ? new OrderRepository(_context) : _orders;
            

        public int SaveChanges()
        {
            ApplyAuditInformation();
            return _context.SaveChanges();
          
        }
        private void ApplyAuditInformation()
        {
            var currentUser = _currentUserService.UserName ?? "SYSTEM";

            var entries = _context.ChangeTracker.Entries().Where(x => x.Entity is IAuditableEntity);

            foreach (var entry in entries)
            {
                var entity = (IAuditableEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedBy = currentUser;
                }

                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = currentUser;

                    entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;

                    entry.Property(nameof(IAuditableEntity.CreatedBy)).IsModified = false;
                }
            }
        }
    }
}
