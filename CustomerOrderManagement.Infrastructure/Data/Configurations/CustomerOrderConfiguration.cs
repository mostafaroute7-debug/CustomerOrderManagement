
using CustomerOrderManagement.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CustomerOrderManagement.Infrastructure.Data.Configurations
{
    internal class CustomerOrderConfiguration : EntityTypeConfiguration<CustomerOrder>
    {
        public CustomerOrderConfiguration()
        {
            ToTable("CustomerOrders");

            HasKey(x => new
            {
                x.CustomerId,
                x.OrderId
            });

            HasRequired(x => x.Customer)
                .WithMany(x => x.CustomerOrders)
                .HasForeignKey(x => x.CustomerId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Order)
                .WithMany(x => x.CustomerOrders)
                .HasForeignKey(x => x.OrderId)
                .WillCascadeOnDelete(false);
        }
    }
}
