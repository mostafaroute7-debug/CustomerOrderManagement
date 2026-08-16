using System;
using System.Collections.Generic;

namespace CustomerOrderManagement.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors): base("Validation failed.")
        {
            Errors = errors;
        }
    }
}
