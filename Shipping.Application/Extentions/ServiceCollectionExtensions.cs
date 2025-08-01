using Microsoft.Extensions.DependencyInjection;
using Shipping.Application.Abstraction;
using Shipping.Application.Services;

namespace Shipping.Application.Extentions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddAutoMapper(applicationAssembly);

        services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));
    }
}
