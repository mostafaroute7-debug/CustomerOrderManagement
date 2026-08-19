using CustomerOrderManagement.Domain.Entities;

namespace CustomerOrderManagement.Infrastructure.Data.Configurations
{
    internal class OrderConfiguration : BaseEntityConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Orders");

            Property(x => x.CreatedAt)
                .HasColumnName("OrderDate");

            Property(x => x.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            Property(x => x.Status)
                .IsRequired();
        }
    }
}
