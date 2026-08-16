using CustomerOrderManagement.API.ExceptionHandling;
using CustomerOrderManagement.API.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace CustomerOrderManagement.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {


            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.MessageHandlers.Add(new RequestResponseLoggingHandler());

            config.Services.Replace(typeof(IExceptionHandler),new GlobalExceptionHandler());


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
