using AutoMapper;
using CustomerOrderManagement.Application.DTOs.Orders;
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
    public class OrderService : IOrderService
    {
         private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderDto> _createValidator;
        private readonly IValidator<UpdateOrderDto> _updateValidator;

        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreateOrderDto> createValidator,
            IValidator<UpdateOrderDto> updateValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public ResultDto<PagedResultDto<OrderDto>> GetAll(PaginationRequest request)
        {
            var query = _unitOfWork.Orders.GetAll();

            var totalCount = query.Count();

            var orders = query
                .OrderByDescending(x => x.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var orderDtos = _mapper.Map<List<OrderDto>>(orders);

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / request.PageSize);

            var result = new PagedResultDto<OrderDto>
            {
                Items = orderDtos,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = request.PageNumber > 1,
                HasNextPage = request.PageNumber < totalPages
            };

            return new ResultDto<PagedResultDto<OrderDto>>
            {
                Success = true,
                Message = "Orders retrieved successfully.",
                Data = result
            };
        }


        public ResultDto<OrderDto> GetById(int id)
        {
            var order = _unitOfWork.Orders
                .GetByIdWithCustomers(id);

            if (order == null)
            {
                return new ResultDto<OrderDto>
                {
                    Success = false,
                    Message = "Order not found.",
                    ErrorCode = "ORDER_NOT_FOUND"
                };
            }

            var orderDto = _mapper.Map<OrderDto>(order);

            return new ResultDto<OrderDto>
            {
                Success = true,
                Message = "Order retrieved successfully.",
                Data = orderDto
            };
        }

        public ResultDto<OrderDto> Create(CreateOrderDto request)
        {
            var validationResult = _createValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new ResultDto<OrderDto>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList()
                };
            }

            var order = _mapper.Map<Order>(request);

            var customerIds = request.CustomerIds
                .Distinct()
                .ToList();

            foreach (var customerId in customerIds)
            {
                order.CustomerOrders.Add(
                    new CustomerOrder
                    {
                        CustomerId = customerId
                    });
            }

            _unitOfWork.Orders.Add(order);

            _unitOfWork.SaveChanges();

            var createdOrder = _unitOfWork.Orders
                .GetByIdWithCustomers(order.Id);

            return new ResultDto<OrderDto>
            {
                Success = true,
                Message = "Order created successfully.",
                Data = _mapper.Map<OrderDto>(createdOrder)
            };
        }

        public ResultDto<OrderDto> Update(int id,UpdateOrderDto request)
        {
            var order = _unitOfWork.Orders
                .GetByIdWithCustomers(id);

            if (order == null)
            {
                return new ResultDto<OrderDto>
                {
                    Success = false,
                    Message = "Order not found.",
                    ErrorCode = "ORDER_NOT_FOUND"
                };
            }

            var validationResult = _updateValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                return new ResultDto<OrderDto>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = validationResult.Errors
                        .Select(x => x.ErrorMessage)
                        .ToList()
                };
            }

            _mapper.Map(request, order);

            var newCustomerIds = request.CustomerIds
                .Distinct()
                .ToHashSet();

            var currentCustomerIds = order.CustomerOrders
                .Select(x => x.CustomerId)
                .ToHashSet();

            var customerIdsToRemove = currentCustomerIds
                .Except(newCustomerIds)
                .ToList();

            foreach (var customerId in customerIdsToRemove)
            {
                var customerOrder = order.CustomerOrders
                    .First(x => x.CustomerId == customerId);

                order.CustomerOrders.Remove(customerOrder);
            }

            var customerIdsToAdd = newCustomerIds
                .Except(currentCustomerIds)
                .ToList();

            foreach (var customerId in customerIdsToAdd)
            {
                order.CustomerOrders.Add(
                    new CustomerOrder
                    {
                        OrderId = order.Id,
                        CustomerId = customerId
                    });
            }

            _unitOfWork.Orders.Update(order);

            _unitOfWork.SaveChanges();

            var updatedOrder = _unitOfWork.Orders
                .GetByIdWithCustomers(id);

            return new ResultDto<OrderDto>
            {
                Success = true,
                Message = "Order updated successfully.",
                Data = _mapper.Map<OrderDto>(updatedOrder)
            };
        }

        public ResultDto<bool> Delete(int id)
        {
            var order = _unitOfWork.Orders
                .GetByIdWithCustomers(id);

            if (order == null)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "Order not found.",
                    ErrorCode = "ORDER_NOT_FOUND",
                    Data = false
                };
            }

            foreach (var customerOrder in order.CustomerOrders.ToList())
            {
                order.CustomerOrders.Remove(customerOrder);
            }

            _unitOfWork.Orders.Delete(order);

            _unitOfWork.SaveChanges();

            return new ResultDto<bool>
            {
                Success = true,
                Message = "Order deleted successfully.",
                Data = true
            };
        }

    }
}
