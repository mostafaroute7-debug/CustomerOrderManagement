using CustomerOrderManagement.Application.DTOs.Authentication;
using CustomerOrderManagement.Domain.Entities;

namespace CustomerOrderManagement.Application.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        LoginResponseDto GenerateToken(ApplicationUser user,string role);
    }
}
