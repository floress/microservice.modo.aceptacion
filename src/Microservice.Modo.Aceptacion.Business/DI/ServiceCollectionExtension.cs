using Microsoft.Extensions.DependencyInjection;

namespace Microservice.Modo.Aceptacion.Business.DI;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAfipBusiness(this IServiceCollection services)
    {

        services.AddLazyCache();

        return services;
    }
}