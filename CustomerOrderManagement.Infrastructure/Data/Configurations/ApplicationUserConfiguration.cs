using CustomerOrderManagement.Infrastructure.Identity;
using System.Data.Entity.ModelConfiguration;

namespace CustomerOrderManagement.Infrastructure.Data.Configurations
{
    internal class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfiguration()
        {
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
