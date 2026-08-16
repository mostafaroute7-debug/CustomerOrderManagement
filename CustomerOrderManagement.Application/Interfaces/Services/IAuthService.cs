using CustomerOrderManagement.Application.DTOs.Authentication;
using CustomerOrderManagement.Application.Results;

namespace CustomerOrderManagement.Application.Interfaces.Services
{
    public interface IAuthService
    {
        ResultDto<LoginResponseDto> Register(RegisterDto request);

        ResultDto<LoginResponseDto> Login(LoginDto request);
    }
}
