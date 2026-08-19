using AutoMapper;
using CustomerOrderManagement.Application.DTOs.Customers;
using CustomerOrderManagement.Application.Exceptions;
using CustomerOrderManagement.Application.Interfaces;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Pagination;
using CustomerOrderManagement.Application.Results;
using CustomerOrderManagement.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOrderManagement.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateCustomerDto> _createValidator;
        private readonly IValidator<UpdateCustomerDto> _updateValidator;

        public CustomerService(IUnitOfWork unitOfWork,IMapper mapper, IValidator<CreateCustomerDto> createValidator, IValidator<UpdateCustomerDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public ResultDto<PagedResultDto<CustomerDto>> GetAll(PaginationRequest request)
        {
            var query = _unitOfWork.Customers.GetAll();
            if (query == null || !query.Any())
            {
                return new ResultDto<PagedResultDto<CustomerDto>>
                {
                    Success = false,
                    Message = "Customers not found.",
                    Data = null
                };
            }
            var totalCount = query.Count();

            var customers = query
                .OrderBy(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var result = new PagedResultDto<CustomerDto>
            {
                Items = customerDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = request.PageNumber > 1,
                HasNextPage = request.PageNumber < totalPages
            };

            return new ResultDto<PagedResultDto<CustomerDto>>
            {
                Success = true,
                Message = "Customers retrieved successfully.",
                Data = result
            };
        }

        public ResultDto<CustomerDto> GetById(int id)
        {
            var customer = _unitOfWork.Customers.GetById(id);

            if (customer == null)
            {
                throw new NotFoundException("Customer not found.","CUSTOMER_NOT_FOUND");
            }

            var dto = _mapper.Map<CustomerDto>(customer);

            return new ResultDto<CustomerDto>
            {
                Success = true,
                Message = "Customer retrieved successfully.",
                Data = dto
            };
        }

        public ResultDto<CustomerDto> Create(CreateCustomerDto request)
        {
            var validationResult = _createValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new Exceptions.ValidationException(
                    validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList());
            }

            var customer = _mapper.Map<Customer>(request);

            _unitOfWork.Customers.Add(customer);

            _unitOfWork.SaveChanges();

            var dto = _mapper.Map<CustomerDto>(customer);

            return new ResultDto<CustomerDto>
            {
                Success = true,
                Message = "Customer created successfully.",
                Data = dto
            };
        }

        public ResultDto<CustomerDto> Update(int id,UpdateCustomerDto request)
        {
            var customer = _unitOfWork.Customers.GetById(id);

            if (customer == null)
            {
                throw new NotFoundException("Customer not found.","CUSTOMER_NOT_FOUND");
            }
            var validationResult = _updateValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                throw new Exceptions.ValidationException(
                    validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList());
            }

            _mapper.Map(request, customer);

            _unitOfWork.Customers.Update(customer);

            _unitOfWork.SaveChanges();

            var dto = _mapper.Map<CustomerDto>(customer);

            return new ResultDto<CustomerDto>
            {
                Success = true,
                Message = "Customer updated successfully.",
                Data = dto
            };
        }

        public ResultDto<bool> Delete(int id)
        {
            var customer = _unitOfWork.Customers.GetById(id);

            if (customer == null)
            {
                throw new NotFoundException("Customer not found.","CUSTOMER_NOT_FOUND");
            }

            _unitOfWork.Customers.Delete(customer);

            _unitOfWork.SaveChanges();

            return new ResultDto<bool>
            {
                Success = true,
                Message = "Customer deleted successfully.",
                Data = true
            };
        }
    }
}
