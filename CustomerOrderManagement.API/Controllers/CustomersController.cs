using CustomerOrderManagement.Application.DTOs.Customers;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Pagination;
using System.Web.Http;

namespace CustomerOrderManagement.API.Controllers
{
    public class CustomersController : ApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll([FromUri] PaginationRequest request)
        {
            var result = _customerService.GetAll(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var result = _customerService.GetById(id);

            if (!result.Success)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CreateCustomerDto request)
        {
            var result = _customerService.Create(request);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Created(
                Request.RequestUri + "/" + result.Data.Id,
                result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id,UpdateCustomerDto request)
        {
            var result = _customerService.Update(
                id,
                request);

            if (!result.Success)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var result = _customerService.Delete(id);

            if (!result.Success)
            {
                return NotFound();
            }

            return StatusCode(
                System.Net.HttpStatusCode.NoContent);
        }
    }
}
