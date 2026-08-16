using CustomerOrderManagement.Application.Results;
using System.Linq;
using System.Web.Http;

namespace CustomerOrderManagement.API.Helpers
{
    public class ValidationResponseHelper
    {
        public static ResultDto<object> Create(ApiController controller)
        {
            var errors = controller.ModelState
                .Where(x => x.Value.Errors.Any())
                .SelectMany(x => x.Value.Errors.Select(e =>
                    string.Format(
                        "{0}: {1}",
                        x.Key,
                        string.IsNullOrWhiteSpace(e.ErrorMessage)? "Invalid value.": e.ErrorMessage))).ToList();

            return new ResultDto<object>
            {
                Success = false,
                Message = "Validation failed",
                Errors = errors
            };
        }
    }
}