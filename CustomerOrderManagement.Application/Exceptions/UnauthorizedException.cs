using System;

namespace CustomerOrderManagement.Application.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized."): base(message)
        {
        }
    }
}
