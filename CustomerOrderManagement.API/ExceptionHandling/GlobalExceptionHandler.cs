using CustomerOrderManagement.Application.Exceptions;
using CustomerOrderManagement.Application.Results;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace CustomerOrderManagement.API.ExceptionHandling
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception;

            var statusCode = HttpStatusCode.InternalServerError;

            var message = "An unexpected error occurred.";

            var errorCode = "INTERNAL_SERVER_ERROR";

            var errors = new List<string>();


            // Validation
            if (exception is ValidationException validationException)
            {
                statusCode = HttpStatusCode.BadRequest;

                message = validationException.Message;

                errorCode = "VALIDATION_ERROR";

                errors = validationException.Errors;
            }


            // Not Found
            else if (exception is NotFoundException notFoundException)
            {
                statusCode = HttpStatusCode.NotFound;

                message = notFoundException.Message;

                errorCode = notFoundException.ErrorCode;
            }


            // Business
            else if (exception is BusinessException businessException)
            {
                statusCode = HttpStatusCode.BadRequest;

                message = businessException.Message;

                errorCode = businessException.ErrorCode;
            }


            // Unauthorized
            else if (exception is UnauthorizedException unauthorizedException)
            {
                statusCode = HttpStatusCode.Unauthorized;

                message = unauthorizedException.Message;

                errorCode = "UNAUTHORIZED";
            }


            // Database
            else if (exception is DbUpdateException)
            {
                statusCode = HttpStatusCode.InternalServerError;

                message = "A database error occurred.";

                errorCode = "DATABASE_ERROR";
            }


            var result = new ResultDto<object>
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode,
                Errors = errors
            };


            context.Result =
                new ResponseMessageResult(
                    context.Request.CreateResponse(
                        statusCode,
                        result));
        }
    }
}