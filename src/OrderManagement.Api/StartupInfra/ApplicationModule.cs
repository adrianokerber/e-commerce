using Autofac;
using OrderManagement.Shared;

namespace OrderManagement.Api.StartupInfra;

public class ApplicationModule : Autofac.Module
{
    public ApplicationModule(){}
    
    protected override void Load(ContainerBuilder builder)
    {
        builder
            .RegisterAssemblyTypes(typeof(Domain.Orders.Order).Assembly)
            .AsClosedTypesOf(typeof(IService<>))
            .InstancePerLifetimeScope();

        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
    }
}