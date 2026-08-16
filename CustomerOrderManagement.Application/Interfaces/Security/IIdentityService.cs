using CustomerOrderManagement.Application.Results;
using CustomerOrderManagement.Domain.Entities;

namespace CustomerOrderManagement.Application.Interfaces.Security
{
    public interface IIdentityService
    {

        ResultDto<ApplicationUser> GetUserByEmail(string email);

        ResultDto<bool> CheckPassword(ApplicationUser user,string password);

        ResultDto<string> GetUserRole(string userId);

        ResultDto<string> CreateUser(string email,string password);
    }
}
