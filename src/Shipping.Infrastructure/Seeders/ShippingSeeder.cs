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
            // Seed Regions
            if (!context.Regions.Any())
            {
                var regions = GetRegions();
                context.Regions.AddRange(regions);
                await context.SaveChangesAsync();
            }

            // Seed CitySettings
            if (!context.CitySettings.Any())
            {
                var regions = context.Regions.ToList();
                var cities = GetCitySettings(regions);
                context.CitySettings.AddRange(cities);
                await context.SaveChangesAsync();
            }

            // Seed Branches
            if (!context.Branches.Any())
            {
                var regions = context.Regions.ToList();
                var branches = GetBranches(regions);
                context.Branches.AddRange(branches);
                await context.SaveChangesAsync();
            }

            // Seed ShippingTypes
            if (!context.ShippingTypes.Any())
            {
                var types = GetShippingTypes();
                context.ShippingTypes.AddRange(types);
                await context.SaveChangesAsync();
            }

            // Seed WeightSettings
            if (!context.WeightSettings.Any())
            {
                var weights = GetWeightSettings();
                context.WeightSettings.AddRange(weights);
                await context.SaveChangesAsync();
            }

            // Seed ApplicationUser
            if (!context.Users.Any())
            {
                var users = GetUsers();
                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // Seed Orders and Products
            if (!context.Orders.Any())
            {
                var branches = context.Branches.ToList();
                var cities = context.CitySettings.ToList();
                var shippingTypes = context.ShippingTypes.ToList();
                var orders = GetOrders(branches, cities, shippingTypes);
                context.Orders.AddRange(orders);
                await context.SaveChangesAsync();
            }

            // Seed OrderReports
            if (!context.OrderReports.Any())
            {
                var orders = context.Orders.ToList();
                var orderReports = GetOrderReports(orders);
                context.OrderReports.AddRange(orderReports);
                await context.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<Region> GetRegions()
    {
        return new List<Region>
        {
            new Region { Governorate = "Cairo", CreatedAt = DateTime.UtcNow },
            new Region { Governorate = "Alexandria", CreatedAt = DateTime.UtcNow },
            new Region { Governorate = "Giza", CreatedAt = DateTime.UtcNow }
        };
    }
    private IEnumerable<Branch> GetBranches(List<Region> regions)
    {
        return new List<Branch>
        {
            new Branch
            {
                Name = "Cairo Main Branch",
                Location = "123 Tahrir Square, Cairo",
                RegionId = regions.First(r => r.Governorate == "Cairo").Id,
                BranchDate = DateTime.UtcNow
            },
            new Branch
            {
                Name = "Alexandria Coastal Branch",
                Location = "456 Corniche Road, Alexandria",
                RegionId = regions.First(r => r.Governorate == "Alexandria").Id,
                BranchDate = DateTime.UtcNow
            }
        };
    }

    private IEnumerable<CitySetting> GetCitySettings(List<Region> regions)
    {
        return new List<CitySetting>
        {
            new CitySetting
            {
                Name = "Downtown Cairo",
                StandardShippingCost = 30.00m,
                PickUpShippingCost = 20.00m,
                RegionId = regions.First(r => r.Governorate == "Cairo").Id,
                CreatedAt = DateTime.UtcNow
            },
            new CitySetting
            {
                Name = "Alexandria Corniche",
                StandardShippingCost = 35.00m,
                PickUpShippingCost = 25.00m,
                RegionId = regions.First(r => r.Governorate == "Alexandria").Id,
                CreatedAt = DateTime.UtcNow
            },
            new CitySetting
            {
                Name = "Giza Pyramids",
                StandardShippingCost = 32.00m,
                PickUpShippingCost = 22.00m,
                RegionId = regions.First(r => r.Governorate == "Giza").Id,
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    private IEnumerable<ShippingType> GetShippingTypes()
    {
        return new List<ShippingType>
        {
            new ShippingType
            {
                Name = "Standard Shipping",
                BaseCost = 25.00m,
                Duration = 3,
                CreatedAt = DateTime.UtcNow
            },
            new ShippingType
            {
                Name = "Express Shipping",
                BaseCost = 50.00m,
                Duration = 1,
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    private IEnumerable<WeightSetting> GetWeightSettings()
    {
        return new List<WeightSetting>
        {
            new WeightSetting
            {
                MinWeight = 0.0m,
                MaxWeight = 5.0m,
                CostPerKg = 5.00m,
                CreatedAt = DateTime.UtcNow
            },
            new WeightSetting
            {
                MinWeight = 5.01m,
                MaxWeight = 10.0m,
                CostPerKg = 4.50m,
                CreatedAt = DateTime.UtcNow
            }
        };
    }

    private IEnumerable<ApplicationUser> GetUsers()
    {
        return new List<ApplicationUser>
    {
        new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "courier1@example.com",
            NormalizedUserName = "COURIER1@EXAMPLE.COM",
            Email = "courier1@example.com",
            NormalizedEmail = "COURIER1@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow
        },
        new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "merchant1@example.com",
            NormalizedUserName = "MERCHANT1@EXAMPLE.COM",
            Email = "merchant1@example.com",
            NormalizedEmail = "MERCHANT1@EXAMPLE.COM",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow
        }
    };
    }

    private IEnumerable<Order> GetOrders(List<Branch> branches, List<CitySetting> cities, List<ShippingType> shippingTypes)
    {
        var merchantId = Guid.NewGuid().ToString();
        var employeeId = Guid.NewGuid().ToString();
        var courierId = Guid.NewGuid().ToString();

        return new List<Order>
        {
            new Order
            {
                TotalWeight = 4.5m,
                OrderCost = 100.00m,
                ShippingCost = 30.00m,
                Notes = "Fragile items",
                CreatedAt = DateTime.UtcNow,
                IsOutOfCityShipping = false,
                Status = OrderStatus.Pending,
                OrderTypes = OrderType.Pickup,
                MerchantId = merchantId,
                EmployeeId = employeeId,
                CourierId = courierId,
                BranchId = branches.First(b => b.Name == "Cairo Main Branch").Id,
                RegionId = (int)branches.First(b => b.Name == "Cairo Main Branch").RegionId,
                CitySettingId = cities.First(c => c.Name == "Downtown Cairo").Id,
                ShippingTypeId = shippingTypes.First(s => s.Name == "Standard Shipping").Id,
                PaymentType = PaymentType.Collectible,
                CustomerName = "John Doe",
                CustomerPhone = "+201234567890",
                CustomerAddress = "123 Nile St, Cairo",
                CustomerEmail = "john.doe@example.com",
                Products = GetProducts().ToList()
            },
            new Order
            {
                TotalWeight = 8.0m,
                OrderCost = 250.00m,
                ShippingCost = 55.00m,
                Notes = "Urgent delivery",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                IsOutOfCityShipping = true,
                Status = OrderStatus.Pending,
                OrderTypes = OrderType.Pickup,
                MerchantId = merchantId,
                EmployeeId = employeeId,
                CourierId = courierId,
                BranchId = branches.First(b => b.Name == "Alexandria Coastal Branch").Id,
                RegionId = (int)branches.First(b => b.Name == "Alexandria Coastal Branch").RegionId,
                CitySettingId = cities.First(c => c.Name == "Alexandria Corniche").Id,
                ShippingTypeId = shippingTypes.First(s => s.Name == "Express Shipping").Id,
                PaymentType = PaymentType.Expulsion,
                CustomerName = "Jane Smith",
                CustomerPhone = "+209876543210",
                CustomerAddress = "789 Coastal Rd, Alexandria",
                CustomerEmail = "jane.smith@example.com",
                Products = GetProducts().Skip(2).Take(2).ToList()
            }
        };
    }

    private IEnumerable<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product { Name = "Book Set", Quantity = 2, Weight = 2.0m, CreatedAt = DateTime.UtcNow },
            new Product { Name = "Electronics", Quantity = 1, Weight = 2.5m, CreatedAt = DateTime.UtcNow },
            new Product { Name = "Clothing Bundle", Quantity = 3, Weight = 3.0m, CreatedAt = DateTime.UtcNow },
            new Product { Name = "Accessories", Quantity = 2, Weight = 2.0m, CreatedAt = DateTime.UtcNow }
        };
    }

    private IEnumerable<OrderReport> GetOrderReports(List<Order> orders)
    {
        return new List<OrderReport>
        {
            new OrderReport
            {
                OrderId = orders.First(o => o.CustomerName == "John Doe").Id,
                ReportDetails = "Order received and processed successfully",
                ReportDate = DateTime.UtcNow
            },
            new OrderReport
            {
                OrderId = orders.First(o => o.CustomerName == "Jane Smith").Id,
                ReportDetails = "Order dispatched to courier",
                ReportDate = DateTime.UtcNow.AddHours(-12)
            }
        };
    }

    //private IEnumerable<CourierReport> GetCourierReports(List<Order> orders)
    //{
    //    var courierId = Guid.NewGuid().ToString();
    //    return new List<CourierReport>
    //    {
    //        new CourierReport
    //        {
    //            OrderId = orders.First(o => o.CustomerName == "John Doe").Id,
    //            CourierId = courierId,
    //            CreatedAt = DateTime.UtcNow
    //        },
    //        new CourierReport
    //        {
    //            OrderId = orders.First(o => o.CustomerName == "Jane Smith").Id,
    //            CourierId = courierId,
    //            CreatedAt = DateTime.UtcNow.AddHours(-12)
    //        }
    //    };
    //}

    //private IEnumerable<SpecialCourierRegion> GetSpecialCourierRegions(List<Region> regions)
    //{
    //    var courierId = Guid.NewGuid().ToString();
    //    return new List<SpecialCourierRegion>
    //    {
    //        new SpecialCourierRegion
    //        {
    //            RegionId = regions.First(r => r.Governorate == "Cairo").Id,
    //            CourierId = courierId,
    //        },
    //        new SpecialCourierRegion
    //        {
    //            RegionId = regions.First(r => r.Governorate == "Alexandria").Id,
    //            CourierId = courierId,
    //        }
    //    };
    //}


    //private IEnumerable<SpecialCityCost> GetSpecialCityCosts(List<CitySetting> cities)
    //{
    //    var merchantId = Guid.NewGuid().ToString();
    //    return new List<SpecialCityCost>
    //    {
    //        new SpecialCityCost
    //        {
    //            CitySettingId = cities.First(c => c.Name == "Downtown Cairo").Id,
    //            MerchantId = merchantId,
    //            Price = 15.00m,
    //            CreatedAt = DateTime.UtcNow
    //        },
    //        new SpecialCityCost
    //        {
    //            CitySettingId = cities.First(c => c.Name == "Alexandria Corniche").Id,
    //            MerchantId = merchantId,
    //            Price = 20.00m,
    //            CreatedAt = DateTime.UtcNow
    //        }
    //    };
    //}
}
