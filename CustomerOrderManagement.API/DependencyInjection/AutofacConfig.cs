using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using CustomerOrderManagement.Application.Authentication;
using CustomerOrderManagement.Application.Interfaces;
using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Application.Interfaces.Security;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Mapping;
using CustomerOrderManagement.Application.Services;
using CustomerOrderManagement.Application.Validators.Customers;
using CustomerOrderManagement.Domain.Entities;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using CustomerOrderManagement.Infrastructure.Data.Repositories;
using CustomerOrderManagement.Infrastructure.Identity;
using CustomerOrderManagement.Infrastructure.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Reflection;
using System.Web.Http;

namespace CustomerOrderManagement.API.DependencyInjection
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(
                Assembly.GetExecutingAssembly());


            builder.RegisterType<ApplicationDbContext>()
                .InstancePerRequest();


            builder.RegisterType<CustomerRepository>()
                .As<ICustomerRepository>()
                .InstancePerRequest();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .InstancePerRequest();


            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();


            builder.RegisterType<CustomerService>()
                .As<ICustomerService>()
                .InstancePerRequest();

            builder.RegisterType<OrderService>()
                .As<IOrderService>()
                .InstancePerRequest();


            builder.RegisterAssemblyTypes(
                    typeof(CreateCustomerValidator).Assembly)
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerRequest();


            var mapperConfiguration =
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                });

            builder.RegisterInstance(mapperConfiguration)
                .As<MapperConfiguration>()
                .SingleInstance();

            builder.RegisterInstance(mapperConfiguration.CreateMapper())
                .As<IMapper>()
                .SingleInstance();

            builder.Register(c =>
            {
                var context = c.Resolve<ApplicationDbContext>();

                return new UserStore<ApplicationUser>(context);
            })
         .As<IUserStore<ApplicationUser>>()
         .InstancePerRequest();

            builder.RegisterType<ApplicationUserManager>()
                    .AsSelf()
                    .As<UserManager<ApplicationUser>>()
                    .InstancePerRequest();

            builder.RegisterType<IdentityService>()
             .As<IIdentityService>()
             .InstancePerRequest();

            builder.RegisterType<AuthService>()
    .As<IAuthService>()
    .InstancePerRequest();
         
            builder.RegisterType<JwtTokenGenerator>()
              .As<IJwtTokenGenerator>()
              .InstancePerRequest();

            var jwtSettings = new JwtSettings
            {
                Secret = ConfigurationManager.AppSettings["Jwt:Secret"],
                Issuer = ConfigurationManager.AppSettings["Jwt:Issuer"],
                Audience = ConfigurationManager.AppSettings["Jwt:Audience"],
                ExpirationMinutes = int.Parse(ConfigurationManager.AppSettings["Jwt:ExpirationMinutes"])
            };

            builder.RegisterInstance(jwtSettings)
                .As<JwtSettings>()
                .SingleInstance();
            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(container);
        }
    }
}