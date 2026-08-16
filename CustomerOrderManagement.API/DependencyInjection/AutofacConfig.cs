using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using CustomerOrderManagement.Application.Interfaces;
using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Application.Interfaces.Services;
using CustomerOrderManagement.Application.Mapping;
using CustomerOrderManagement.Application.Services;
using CustomerOrderManagement.Application.Validators.Customers;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using CustomerOrderManagement.Infrastructure.Data.Repositories;
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

            builder.RegisterInstance(
                    mapperConfiguration.CreateMapper())
                .As<IMapper>()
                .SingleInstance();


            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver =
                new AutofacWebApiDependencyResolver(container);
        }
    }
}