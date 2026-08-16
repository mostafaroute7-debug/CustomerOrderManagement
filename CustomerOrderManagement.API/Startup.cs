using CustomerOrderManagement.API;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Configuration;
using System.Security.Claims;
using System.Text;

[assembly: OwinStartup(typeof(Startup))]

namespace CustomerOrderManagement.API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var secret = ConfigurationManager.AppSettings["Jwt:Secret"];

            var issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];

            var audience = ConfigurationManager.AppSettings["Jwt:Audience"];

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,

                    TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = issuer,

                            ValidateAudience = true,
                            ValidAudience = audience,

                            ValidateIssuerSigningKey = true,

                            IssuerSigningKey =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),

                            ValidateLifetime = true,

                            RoleClaimType = ClaimTypes.Role,

                            NameClaimType = ClaimTypes.Name
                        }
                });
        }
    }
}