using CustomerOrderManagement.Application.DTOs.Customers;
using CustomerOrderManagement.Application.Interfaces.Repositories;
using FluentValidation;

namespace CustomerOrderManagement.Application.Validators.Customers
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public UpdateCustomerValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .Must((dto, email) => BeUniqueEmail(email, dto.Id))
                .WithMessage("Email already exists.");

            RuleFor(x => x.Phone)
                .Matches(@"^\+201[0-9]{9}$")
                .WithMessage("Phone must be a valid Egyptian mobile number.")
                .Must((dto, phone) => BeUniquePhone(phone, dto.Id))
                .WithMessage("Phone already exists.");
        }

        private bool BeUniqueEmail(string email, int customerId)
        {
            var customer = _customerRepository.GetByEmail(email);

            return customer == null || customer.Id == customerId;
        }

        private bool BeUniquePhone(string phone, int customerId)
        {
            var customer = _customerRepository.GetByPhone(phone);

            return customer == null || customer.Id == customerId;
        }
    }
}
