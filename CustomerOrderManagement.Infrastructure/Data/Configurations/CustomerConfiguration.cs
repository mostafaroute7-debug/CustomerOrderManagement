using CustomerOrderManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;

namespace CustomerOrderManagement.Infrastructure.Data.Configurations
{
    internal class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            ToTable("Customers");

            Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(50);

            Property(x => x.Address)
                .HasMaxLength(250);

            Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                new IndexAttribute("IX_Customers_Phone")
                {
                    IsUnique = true
                }));

            Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnAnnotation(
                IndexAnnotation.AnnotationName,
                new IndexAnnotation(
                new IndexAttribute("IX_Customers_Email")
                {
                    IsUnique = true
                }));

        }
    }
}
