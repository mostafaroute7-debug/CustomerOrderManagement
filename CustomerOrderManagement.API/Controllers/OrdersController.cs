using CustomerOrderManagement.API.Helpers;
using CustomerOrderManagement.Application.DTOs.Orders;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Pagination;
using System.Net;
using System.Web.Http;

namespace CustomerOrderManagement.API.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll([FromUri] PaginationRequest request)
        {
            var result = _orderService.GetAll(request);

            if (!result.Success)
                return Content(HttpStatusCode.BadRequest,result);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var result = _orderService.GetById(id);

            if (!result.Success)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(CreateOrderDto request)
        {
            if (!ModelState.IsValid)
            {
                var validationResult = ValidationResponseHelper.Create(this);

                return Content(HttpStatusCode.BadRequest,validationResult);
            }
            var result = _orderService.Create(request);

            if (!result.Success)
                return Content(HttpStatusCode.BadRequest, result);

            return Created(Request.RequestUri + "/" + result.Data.Id,result);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult Update(int id,UpdateOrderDto request)
        {
            if (!ModelState.IsValid)
            {
                var validationResult =
                    ValidationResponseHelper.Create(this);

                return Content(HttpStatusCode.BadRequest,validationResult);
            }
            var result = _orderService.Update(id, request);

            if (!result.Success)
            {
                if (result.ErrorCode == "ORDER_NOT_FOUND")
                    return NotFound();

                return Content(HttpStatusCode.BadRequest, result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var result = _orderService.Delete(id);

            if (!result.Success)
                return NotFound();

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}