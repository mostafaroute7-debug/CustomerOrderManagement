using CustomerOrderManagement.Application.DTOs.Customers;
using CustomerOrderManagement.Application.Pagination;
using CustomerOrderManagement.Application.Results;

namespace CustomerOrderManagement.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        ResultDto<PagedResultDto<CustomerDto>> GetAll(PaginationRequest request);
        ResultDto<CustomerDto> GetById(int id);
        ResultDto<CustomerDto> Create(CreateCustomerDto request);
        ResultDto<CustomerDto> Update(int id,UpdateCustomerDto request);
        ResultDto<bool> Delete(int id);
    }
}
