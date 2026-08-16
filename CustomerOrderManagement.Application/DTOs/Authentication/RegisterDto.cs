using System.ComponentModel.DataAnnotations;

namespace CustomerOrderManagement.Application.DTOs.Authentication
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 8)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
