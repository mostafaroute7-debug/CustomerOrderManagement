using CustomerOrderManagement.Application.DTOs.Orders;
using CustomerOrderManagement.Application.Interfaces;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOrderManagement.Application.Validators.Orders
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.CustomerIds)
                .NotNull()
                .WithMessage("CustomerIds are required.");

            RuleFor(x => x.CustomerIds)
                .Must(x => x != null && x.Any())
                .WithMessage("At least one customer is required.");

            RuleForEach(x => x.CustomerIds)
                .GreaterThan(0)
                .WithMessage("Customer ID must be greater than zero.");

            RuleFor(x => x.CustomerIds)
                .Must(HaveUniqueCustomerIds)
                .WithMessage("Customer IDs must be unique.");

            RuleFor(x => x.CustomerIds)
                .Must(AllCustomersExist)
                .WithMessage("One or more customers were not found.");
        }

        private bool HaveUniqueCustomerIds(List<int> customerIds)
        {
            if (customerIds == null)
                return true;

            return customerIds.Distinct().Count() == customerIds.Count;
        }

        private bool AllCustomersExist(
            List<int> customerIds)
        {
            if (customerIds == null || !customerIds.Any())
                return true;

            var existingCount = _unitOfWork.Customers.GetAll().Count(x => customerIds.Contains(x.Id));

            return existingCount == customerIds.Distinct().Count();
        }
    }
}
