using CustomerOrderManagement.Application.DTOs.Orders;
using CustomerOrderManagement.Application.Pagination;
using CustomerOrderManagement.Application.Results;

namespace CustomerOrderManagement.Application.Interfaces.Services
{
    public interface IOrderService
    {
        ResultDto<PagedResultDto<OrderDto>> GetAll(PaginationRequest request);
        ResultDto<OrderDto> GetById(int id);
        ResultDto<OrderDto> Create(CreateOrderDto request);
        ResultDto<OrderDto> Update(int id,UpdateOrderDto request);
        ResultDto<bool> Delete(int id);
    }
}
