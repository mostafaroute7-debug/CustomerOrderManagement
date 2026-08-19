using CustomerOrderManagement.Application.Interfaces.Security;
using CustomerOrderManagement.Application.Results;
using CustomerOrderManagement.Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;

namespace CustomerOrderManagement.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ResultDto<ApplicationUser> GetUserByEmail(string email)
        {
            var user = _userManager.FindByEmail(email);

            if (user == null)
            {
                return new ResultDto<ApplicationUser>
                {
                    Success = false,
                    Message = "User not found.",
                    ErrorCode = "USER_NOT_FOUND"
                };
            }

            return new ResultDto<ApplicationUser>
            {
                Success = true,
                Message = "User found.",
                Data = user
            };
        }

        public ResultDto<bool> CheckPassword(ApplicationUser user,string password)
        {
            if (user == null)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    ErrorCode = "INVALID_CREDENTIALS",
                    Data = false
                };
            }

            var valid = _userManager.CheckPassword(user, password);

            if (!valid)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "Invalid email or password.",
                    ErrorCode = "INVALID_CREDENTIALS",
                    Data = false
                };
            }

            return new ResultDto<bool>
            {
                Success = true,
                Message = "Password is valid.",
                Data = true
            };
        }

        public ResultDto<string> CreateUser(string email,string password)
        {
            var existingUser = _userManager.FindByEmail(email);

            if (existingUser != null)
            {
                return new ResultDto<string>
                {
                    Success = false,
                    Message = "Email already exists.",
                    ErrorCode = "EMAIL_ALREADY_EXISTS"
                };
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email,
                EmailConfirmed = true,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            var result = _userManager.Create(user, password);

            if (!result.Succeeded)
            {
                return new ResultDto<string>
                {
                    Success = false,
                    Message = "User registration failed.",
                    Errors = result.Errors.ToList()
                };
            }

            var roleResult =  _userManager.AddToRole(user.Id,"user");

            if (!roleResult.Succeeded)
            {
                _userManager.Delete(user);

                return new ResultDto<string>
                {
                    Success = false,
                    Message = "Failed to assign user role.",
                    Errors = roleResult.Errors.ToList()
                };
            }

            return new ResultDto<string>
            {
                Success = true,
                Message = "User created successfully.",
                Data = user.Id
            };
        }

        public ResultDto<string> GetUserRole(string userId)
        {
            var role = _userManager.GetRoles(userId).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(role))
            {
                return new ResultDto<string>
                {
                    Success = false,
                    Message = "User role was not found.",
                    ErrorCode = "ROLE_NOT_FOUND"
                };
            }

            return new ResultDto<string>
            {
                Success = true,
                Message = "User role retrieved successfully.",
                Data = role
            };
        }
    }
}
