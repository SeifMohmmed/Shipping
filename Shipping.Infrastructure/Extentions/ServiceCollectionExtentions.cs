using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shipping.Application.Abstraction.Dashboard;
using Shipping.Application.Abstraction.Roles;
using Shipping.Domain.Entities;
using Shipping.Domain.Helpers;
using Shipping.Domain.Repositories;
using Shipping.Infrastructure.DashboardServices;
using Shipping.Infrastructure.Persistence;
using Shipping.Infrastructure.Repositories;
using Shipping.Infrastructure.RoleServices;
using Shipping.Infrastructure.Seeders;

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


        services.AddIdentityApiEndpoints<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();


        services.AddAuthorization(options =>
        {
            foreach (var permission in Permissions.GetAllPermissions())
            {
                if (permission is not null)
                {
                    options.AddPolicy(permission, policy =>
                        policy.RequireClaim(Permissions.Type, permission));
                }
            }
        });


        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IShippingSeeder, ShippingSeeder>();

    }
}
