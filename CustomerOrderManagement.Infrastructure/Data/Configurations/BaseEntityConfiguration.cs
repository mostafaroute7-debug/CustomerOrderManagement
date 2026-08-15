using CustomerOrderManagement.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CustomerOrderManagement.Infrastructure.Data.Configurations
{
    internal class BaseEntityConfiguration<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        protected BaseEntityConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.CreatedAt)
                .IsRequired();

            Property(x => x.CreatedBy)
                .HasMaxLength(50)
                .IsRequired();

            Property(x => x.UpdatedAt)
                .IsOptional();

            Property(x => x.UpdatedBy)
                .HasMaxLength(50)
                .IsOptional();
        }
    }
}
