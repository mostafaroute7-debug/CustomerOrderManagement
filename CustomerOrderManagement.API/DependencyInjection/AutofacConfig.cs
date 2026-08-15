using Autofac;
using Autofac.Integration.WebApi;
using CustomerOrderManagement.Application.Interfaces.Repositories;
using CustomerOrderManagement.Application.Interfaces;
using CustomerOrderManagement.Infrastructure.Data.Contexts;
using System.Reflection;
using System.Web.Http;
using CustomerOrderManagement.Infrastructure.Data.Repositories;
namespace CustomerOrderManagement.API.DependencyInjection
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

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

            var container = builder.Build();

            config.DependencyResolver =
                new AutofacWebApiDependencyResolver(container);
        }
    }
}