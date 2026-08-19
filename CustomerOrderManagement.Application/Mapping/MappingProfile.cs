using AutoMapper;
using CustomerOrderManagement.Application.DTOs.Customers;
using CustomerOrderManagement.Application.DTOs.Orders;
using CustomerOrderManagement.Domain.Entities;

namespace CustomerOrderManagement.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();

            CreateMap<CreateCustomerDto, Customer>();

            CreateMap<UpdateCustomerDto, Customer>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Order, OrderDto>();

            CreateMap<CreateOrderDto, Order>().ForMember( dest => dest.CustomerOrders,opt => opt.Ignore());

            CreateMap<UpdateOrderDto, Order>().ForMember(dest => dest.CustomerOrders,opt => opt.Ignore());
        }
    }
}
