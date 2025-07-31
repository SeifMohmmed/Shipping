using Shipping.API.Extensions;
using Shipping.Application.Extentions;
using Shipping.Infrastructure.Extentions;
using Shipping.Infrastructure.Seeders;
namespace Shipping.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.AddPresentation();

            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddApplication();

            var app = builder.Build();

            //Seeding
            var scoope = app.Services.CreateScope();
            var seeder = scoope.ServiceProvider.GetRequiredService<IShippingSeeder>();
            seeder.SeedAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
