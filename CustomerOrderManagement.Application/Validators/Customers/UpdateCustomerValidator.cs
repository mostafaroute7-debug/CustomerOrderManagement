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
            RuleFor(x => x.Email)
                  .Must((dto, email) => BeUniqueEmail(email, dto.Id))
            .EmailAddress()
            .WithMessage("Invalid email format.");


            RuleFor(x => x.Phone)
                 .Must((dto, phone) => BeUniquePhone(phone, dto.Id))
                .Matches(@"^\+201[0-9]{9}$")
                .WithMessage("Phone must be a valid Egyptian mobile number.");
        }
        private bool BeUniqueEmail(string email,int customerId)
        {
            var customer =_customerRepository.GetByEmail(email);

            return customer == null || customer.Id == customerId;
        }

        private bool BeUniquePhone(string phone,int customerId)
        {
            var customer =_customerRepository.GetByPhone(phone);

            return customer == null || customer.Id == customerId;
        }
    }
}
