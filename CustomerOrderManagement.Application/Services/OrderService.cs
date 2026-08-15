using AutoMapper;
using CustomerOrderManagement.Application.DTOs.Orders;
using CustomerOrderManagement.Application.Interfaces;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Pagination;
using CustomerOrderManagement.Application.Results;
using CustomerOrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerOrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
         private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ResultDto<PagedResultDto<OrderDto>> GetAll(
            PaginationRequest request)
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
            var customerIds = request.CustomerIds
                .Distinct()
                .ToList();

            var customers = _unitOfWork.Customers
                .GetAll()
                .Where(x => customerIds.Contains(x.Id))
                .ToList();

            if (customers.Count != customerIds.Count)
            {
                var existingIds = customers
                    .Select(x => x.Id)
                    .ToHashSet();

                var missingIds = customerIds
                    .Where(x => !existingIds.Contains(x))
                    .ToList();

                return new ResultDto<OrderDto>
                {
                    Success = false,
                    Message = "One or more customers were not found.",
                    ErrorCode = "CUSTOMER_NOT_FOUND",
                    Errors = missingIds
                        .Select(x =>
                            $"Customer with ID {x} was not found.")
                        .ToList()
                };
            }

            var order = _mapper.Map<Order>(request);

            foreach (var customer in customers)
            {
                order.CustomerOrders.Add(
                    new CustomerOrder
                    {
                        CustomerId = customer.Id
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

        public ResultDto<OrderDto> Update(
            int id,
            UpdateOrderDto request)
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

            var newCustomerIds = request.CustomerIds
                .Distinct()
                .ToHashSet();

            var customers = _unitOfWork.Customers
                .GetAll()
                .Where(x => newCustomerIds.Contains(x.Id))
                .ToList();

            if (customers.Count != newCustomerIds.Count)
            {
                var existingIds = customers
                    .Select(x => x.Id)
                    .ToHashSet();

                var missingIds = newCustomerIds
                    .Where(x => !existingIds.Contains(x))
                    .ToList();

                return new ResultDto<OrderDto>
                {
                    Success = false,
                    Message = "One or more customers were not found.",
                    ErrorCode = "CUSTOMER_NOT_FOUND",
                    Errors = missingIds
                        .Select(x =>
                            $"Customer with ID {x} was not found.")
                        .ToList()
                };
            }

            _mapper.Map(request, order);


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

            foreach (var customerOrder in
                     order.CustomerOrders.ToList())
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
