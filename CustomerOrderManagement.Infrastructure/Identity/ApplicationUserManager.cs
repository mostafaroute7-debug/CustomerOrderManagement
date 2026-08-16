
using CustomerOrderManagement.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace CustomerOrderManagement.Infrastructure.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(
           IUserStore<ApplicationUser> store)
           : base(store)
        {
        }
    }
}
