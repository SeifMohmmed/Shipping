using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shipping.Application.Abstraction.Auth;
using Shipping.Application.Abstraction.User;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.Persistence;
using Shipping.Infrastructure.Repositories;
using Shipping.Infrastructure.RoleServices;
using Shipping.Infrastructure.Seeders;
using Shipping.Infrastructure.UserServices;

namespace Shipping.Infrastructure.Extentions;
public static class ServiceCollectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string txt = "";

        //Add Connection to SQL Server
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer(connectionString);
        });

        services.AddCors(options =>
        {
            options.AddPolicy(txt,
            builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });


        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IShippingSeeder, ShippingSeeder>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IRoleService, RoleService>();

    }
}
