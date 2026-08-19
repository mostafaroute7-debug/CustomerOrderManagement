
using CustomerOrderManagement.Application.Interfaces;
using System.Web;

namespace CustomerOrderManagement.API.Helpers
{
    public class CurrentUserService : ICurrentUserService
    {
        public string UserId => HttpContext.Current?.User?.Identity?.IsAuthenticated == true? HttpContext.Current.User.Identity.Name: null;

        public string UserName => HttpContext.Current?.User?.Identity?.IsAuthenticated == true? HttpContext.Current.User.Identity.Name: null;

        public bool IsAuthenticated => HttpContext.Current?.User?.Identity?.IsAuthenticated == true;
    }
}