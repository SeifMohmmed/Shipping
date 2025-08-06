using AutoMapper;
using FluentAssertions;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Application.Mapping;
using Shipping.Domain.Entities;
using Shipping.Domain.Enums;

namespace Shipping.Application.Tests.Mapping;
public class MappingProfileTests
{
    private IMapper _mapper;

    public MappingProfileTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = configuration.CreateMapper();
    }

    [Fact()]
    public void CreateMap_ForProductToProductDTO_MapsCorrectly()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Sample Product A",
            Quantity = 10,
            Weight = 2.5m,
            CreatedAt = new DateTime(2025, 8, 6),
            OrderId = 101
        };

        //Act
        var productDTO = _mapper.Map<ProductDTO>(product);

        //Assert
        productDTO.Should().NotBeNull();
        productDTO.Id.Should().Be(product.Id);
        productDTO.Name.Should().Be(product.Name);
        productDTO.Quantity.Should().Be(product.Quantity);
        productDTO.Weight.Should().Be(product.Weight);
        productDTO.OrderId.Should().Be(product.OrderId);
    }

    [Fact()]
    public void CreateMap_ForProductToUpdateProductDTO_MapsCorrectly()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Sample Product A",
            Quantity = 10,
            Weight = 2.5m,
            CreatedAt = new DateTime(2025, 8, 6),
        };

        //Act
        var productDTO = _mapper.Map<UpdateProductDTO>(product);

        //Assert
        productDTO.Should().NotBeNull();
        productDTO.Name.Should().Be(product.Name);
        productDTO.Quantity.Should().Be(product.Quantity);
        productDTO.Weight.Should().Be(product.Weight);
    }

    [Fact()]
    public void CreateMap_ForRegionToRegionDTO_MapsCorrectly()
    {
        //Arrange
        var region = new Region
        {
            Governorate = "Cairo",
            IsDeleted = false,
            CreatedAt = new DateTime(2025, 8, 6),
            CitySettings = new List<CitySetting>
            {
              new CitySetting { Name = "Nasr City" },
              new CitySetting { Name = "Heliopolis" }
            }
        };

        //Act
        var regionDTO = _mapper.Map<RegionDTO>(region);

        //Assert
        regionDTO.Should().NotBeNull();
        regionDTO.Governorate.Should().Be(region.Governorate);
        regionDTO.IsDeleted.Should().Be(region.IsDeleted);
        regionDTO.CreatedAt.Should().Be(region.CreatedAt);
        regionDTO.CityName.Should().Contain(new[] { "Nasr City", "Heliopolis" });
    }

    [Fact()]
    public void CreateMap_ForCourierReportToCourierReportDTO_MapsCorrectly()
    {
        //Arrange
        var courierReport = new CourierReport
        {
            Id = 1,
            CreatedAt = new DateTime(2025, 8, 6),
            CourierId = "courier-1",
            Courier = new ApplicationUser
            {
                FullName = "Ahmed",
            },
            OrderId = 101,
            Order = new Order
            {
                CustomerName = "John Doe",
                CustomerAddress = "123 Test Street",
                CustomerPhone = "01234567890",
                Status = OrderStatus.Delivered,
                OrderCost = 250.00m,
                MerchantId = "merchant-1",
                Notes = "Leave at door",
                CitySetting = new CitySetting
                {
                    Name = "Nasr City"
                },
                Products = new List<Product>
                {
                new Product { Name = "Product A" },
                new Product { Name = "Product B" }
                }
            }
        };
        //Act
        var regionDTO = _mapper.Map<CourierReportDTO>(courierReport);

        //Assert
        regionDTO.Should().NotBeNull();
        regionDTO.Id.Should().Be(courierReport.Id);
        regionDTO.OrderId.Should().Be(courierReport.OrderId);
        regionDTO.CreatedAt.Should().Be(courierReport.CreatedAt);
        regionDTO.CourierName.Should().Be(courierReport.Courier.FullName);
        regionDTO.Area.Should().Be(courierReport.Order.CitySetting.Name);
        regionDTO.CustomerName.Should().Be(courierReport.Order.CustomerName);
        regionDTO.CustomerAddress.Should().Be(courierReport.Order.CustomerAddress);
        regionDTO.CustomerPhone.Should().Be(courierReport.Order.CustomerPhone);
        regionDTO.products.Should().Contain(courierReport.Order.Products.Select(s => s.Name));
        regionDTO.Notes.Should().Be(courierReport.Order.Notes);
        regionDTO.orderStatus.Should().Be(courierReport.Order.Status.ToString());
        regionDTO.Amount.Should().Be(courierReport.Order.OrderCost);
        regionDTO.MerchantId.Should().Be(courierReport.Order.MerchantId);
    }
}
