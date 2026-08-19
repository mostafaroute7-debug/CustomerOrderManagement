using CustomerOrderManagement.Application.DTOs.Authentication;
using CustomerOrderManagement.Application.Interfaces.Security;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Results;
using CustomerOrderManagement.Domain.Entities;
namespace CustomerOrderManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IIdentityService identityService,IJwtTokenGenerator jwtTokenGenerator)
        {
            _identityService = identityService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public ResultDto<LoginResponseDto> Register(RegisterDto request)
        {
            var createUserResult = _identityService.CreateUser(request.Email,request.Password);

            if (!createUserResult.Success)
            {
                return new ResultDto<LoginResponseDto>
                {
                    Success = false,
                    Message = createUserResult.Message,
                    ErrorCode = createUserResult.ErrorCode,
                    Errors = createUserResult.Errors
                };
            }

            var userResult = _identityService.GetUserByEmail(request.Email);

            if (!userResult.Success || userResult.Data == null)
            {
                return new ResultDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "User was created but could not be retrieved.",
                    ErrorCode = "USER_NOT_FOUND"
                };
            }

            ApplicationUser user = userResult.Data;

            var roleResult = _identityService.GetUserRole(user.Id);

            if (!roleResult.Success ||
                string.IsNullOrWhiteSpace(roleResult.Data))
            {
                return new ResultDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "User role was not assigned.",
                    ErrorCode = "ROLE_NOT_FOUND"
                };
            }

            var token = _jwtTokenGenerator.GenerateToken(user,roleResult.Data);

            return new ResultDto<LoginResponseDto>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = token
            };
        }

        public ResultDto<LoginResponseDto> Login(LoginDto request)
        {
            var userResult =_identityService.GetUserByEmail(request.Email);

            if (!userResult.Success || userResult.Data == null)
            {
                return new ResultDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    ErrorCode = "INVALID_CREDENTIALS"
                };
            }

            ApplicationUser user = userResult.Data;

            var passwordResult = _identityService.CheckPassword(user, request.Password);

            if (!passwordResult.Success)
            {
                return new ResultDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    ErrorCode = "INVALID_CREDENTIALS",
                    Errors = passwordResult.Errors
                };
            }

            var roleResult =_identityService.GetUserRole(user.Id);

            if (!roleResult.Success || string.IsNullOrWhiteSpace(roleResult.Data))
            {
                return new ResultDto<LoginResponseDto>
                {
                    Success = false,
                    Message = "User role was not assigned.",
                    ErrorCode = "ROLE_NOT_FOUND"
                };
            }

            var token = _jwtTokenGenerator.GenerateToken(user,roleResult.Data);

            return new ResultDto<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = token
            };
        }
    }
}
