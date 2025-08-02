using Microsoft.Extensions.DependencyInjection;
using Shipping.Application.Abstraction;
using Shipping.Application.Abstraction.User;
using Shipping.Application.Services;
using Shipping.Application.Services.UserSerivces;
using Shipping.Domain.Entities;

namespace Shipping.Application.Extentions;
public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddAutoMapper(applicationAssembly);

        services.AddScoped(typeof(IServiceManager), typeof(ServiceManager));

        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IUserService, UserService>();

        services.AddHttpContextAccessor();
    }
}
