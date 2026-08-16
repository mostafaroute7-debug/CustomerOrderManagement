using System.ComponentModel.DataAnnotations;

namespace CustomerOrderManagement.Application.DTOs.Customers
{
    public class CreateCustomerDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        public string Phone { get; set; }
    }
}
