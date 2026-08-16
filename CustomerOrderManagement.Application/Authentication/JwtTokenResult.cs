using System;

namespace CustomerOrderManagement.Application.Authentication
{
    public class JwtTokenResult
    {
        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}
