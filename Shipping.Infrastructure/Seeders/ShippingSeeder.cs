using Shipping.Domain.Entities;
using Shipping.Domain.Enums;
using Shipping.Infrastructure.Persistence;

namespace Shipping.Infrastructure.Seeders;
internal class ShippingSeeder(ApplicationDbContext context) : IShippingSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Database.CanConnectAsync())
        {
            if (!context.Orders.Any())
            {
                var orders = GetOrders();
                context.Orders.AddRange(orders);
                await context.SaveChangesAsync();
            }

            if (!context.Regions.Any())
            {
                var regions = GetRegions();
                context.Regions.AddRange(regions);
                await context.SaveChangesAsync();
            }

            if (!context.Branches.Any())
            {
                var branches = GetBranches();
                context.Branches.AddRange(branches);
                await context.SaveChangesAsync();
            }

            if (!context.CitySettings.Any())
            {
                var cities = GetCitySettings();
                context.CitySettings.AddRange(cities);
                await context.SaveChangesAsync();
            }

            if (!context.ShippingTypes.Any())
            {
                var types = GetShippingTypes();
                context.ShippingTypes.AddRange(types);
                await context.SaveChangesAsync();
            }

            if (!context.WeightSettings.Any())
            {
                var weights = GetWeightSettings();
                context.WeightSettings.AddRange(weights);
                await context.SaveChangesAsync();
            }
        }
    }
    private IEnumerable<Region> GetRegions()
    {
        return new List<Region>
        {
            new()
            {
                Governorate = "Cairo",
                CreatedAt = DateTime.Now,
                IsDeleted = false
            }
        };
    }
    private IEnumerable<Order> GetOrders()
    {
        return new List<Order>
    {
        new Order
        {
            TotalWeight = 10.5m,
            OrderCost = 200.00m,
            ShippingCost = 50.00m,
            Notes = "Urgent delivery",
            CreatedAt = DateTime.Now,
            IsOutOfCityShipping = false,
            IsDeleted = false,
            Status = OrderStatus.Pending,
            OrderTypes = OrderType.Pickup,
            MerchantId = "0195d439-9ca1-7873-9c14-a4bc1c201593",
            EmployeeId = "0195d439-9ca1-7873-9c14-a4bc1c201593",
            CourierId = "01961d25-b4da-75a5-a1f4-a7aa10e421ed",
            BranchId = 1,
            RegionId = 1,
            CitySettingId = 1,
            ShippingTypeId = 1,
            PaymentType = PaymentType.Prepaid,
            CustomerName = "Ahmed Ali",
            CustomerPhone = "01012345678",
            CustomerAddress = "123 St, Cairo",
            CustomerEmail = "ahmed@example.com",
            Products = new List<Product>
            {
                new Product
                {
                    Name = "Laptop",
                    Quantity = 1,
                    Weight = 2.5m,
                    CreatedAt = DateTime.Now
                }
            }
        }
    };
    }
    private IEnumerable<Branch> GetBranches()
    {
        return new List<Branch>
        {
            new()
            {
                Name = "Main Branch",
                Location = "Downtown",
                BranchDate = DateTime.Now,
                IsDeleted = false,
                RegionId = 1 // assumes region above has Id=1
            }
        };
    }

    private IEnumerable<CitySetting> GetCitySettings()
    {
        return new List<CitySetting>
        {
            new()
            {
                Name = "Nasr City",
                StandardShippingCost = 50,
                PickUpShippingCost = 30,
                RegionId = 1
            }
        };
    }

    private IEnumerable<ShippingType> GetShippingTypes()
    {
        return new List<ShippingType>
        {
            new()
            {
                Name = "Express",
                BaseCost = 100,
                Duration = 2
            }
        };
    }

    private IEnumerable<WeightSetting> GetWeightSettings()
    {
        return new List<WeightSetting>
        {
            new()
            {
                MinWeight = 0,
                MaxWeight = 20,
                CostPerKg = 10
            }
        };
    }
}
