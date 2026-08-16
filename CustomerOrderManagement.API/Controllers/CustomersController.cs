using CustomerOrderManagement.API.Helpers;
using CustomerOrderManagement.Application.DTOs.Customers;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Pagination;
using System.Net;
using System.Web.Http;

namespace CustomerOrderManagement.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Customers")]
    public class CustomersController : ApiController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Roles = "user,admin")]
        public IHttpActionResult GetAll([FromUri] PaginationRequest request)
        {
            var result = _customerService.GetAll(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "user,admin")]
        public IHttpActionResult GetById(int id)
        {
            var result = _customerService.GetById(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult Create(CreateCustomerDto request)
        {

            if (!ModelState.IsValid)
            {
                var validationResult =ValidationResponseHelper.Create(this);

                return Content(HttpStatusCode.BadRequest,validationResult);
            }
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
        [Authorize(Roles = "admin")]
        public IHttpActionResult Update(int id,UpdateCustomerDto request)
        {
            if (!ModelState.IsValid)
            {
                var validationResult = ValidationResponseHelper.Create(this);

                return Content(HttpStatusCode.BadRequest, validationResult);
            }
            var result = _customerService.Update(id,request);

            if (!result.Success)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult Delete(int id)
        {
            var result = _customerService.Delete(id);

            if (!result.Success)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
