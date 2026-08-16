using CustomerOrderManagement.Application.DTOs.Customers;
using CustomerOrderManagement.Application.Interfaces.Repositories;
using FluentValidation;

namespace CustomerOrderManagement.Application.Validators.Customers
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(x => x.Email)
                .Must(BeUniqueEmail)
                .WithMessage("Email already exists.");

            RuleFor(x => x.Phone)
                  .Must(BeUniquePhone)
                    .Matches(@"^\+201[0-9]{9}$")
                    .WithMessage("Phone must be a valid Egyptian mobile number.");
        }

        private bool BeUniqueEmail(string email)
        {
            return _customerRepository.GetByEmail(email) == null;
        }
        private bool BeUniquePhone(string phone)
        {
            return _customerRepository.GetByPhone(phone) == null;
        }
    }
}
