
using CustomerOrderManagement.Application.DTOs.Authentication;
using CustomerOrderManagement.Application.Interfaces.Services;
using System.Net;
using System.Web.Http;

namespace CustomerOrderManagement.API.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        // POST: api/auth/login
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(LoginDto request)
        {
            if (request == null)
            {
                return Content(
                    HttpStatusCode.BadRequest,
                    new
                    {
                        Success = false,
                        Message = "Request body is required."
                    });
            }

            var result = _authService.Login(request);

            if (!result.Success)
            {
                return Content(
                    HttpStatusCode.Unauthorized,
                    result);
            }

            return Ok(result);
        }

        // POST: api/auth/register
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(RegisterDto request)
        {
            if (request == null)
            {
                return Content(
                    HttpStatusCode.BadRequest,
                    new
                    {
                        Success = false,
                        Message = "Request body is required."
                    });
            }

            var result = _authService.Register(request);

            if (!result.Success)
            {
                return Content(
                    HttpStatusCode.BadRequest,
                    result);
            }

            return Ok(result);
        }
    }
}
