using AutoMapper;
using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Application.Abstraction.SpecialCityCost.DTO;
using Shipping.Application.Abstraction.SpecialCourierRegion.DTO;
using Shipping.Domain.Entities;

namespace Shipping.Application.Mapping;
public class MappingProfile : Profile
{

    public MappingProfile()
    {
        #region Configuration of Product
        CreateMap<Product, ProductDTO>().ReverseMap();

        CreateMap<Product, UpdateProductDTO>().ReverseMap();
        #endregion

        #region Configuration of CourierReport
        CreateMap<CourierReport, CourierReportDTO>()
          .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Order != null && src.Order.CitySetting != null ? src.Order.CitySetting.Name : string.Empty))
          .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Order != null ? src.Order.CustomerName : string.Empty))
          .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Order != null ? src.Order.CustomerPhone : string.Empty))
          .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.Order != null ? src.Order.CustomerAddress : string.Empty))
          .ForMember(dest => dest.products, opt => opt.MapFrom(src => src.Order != null && src.Order.Products != null
             ? src.Order.Products.Select(x => x.Name).ToList()
             : new List<string>()))
          .ForMember(dest => dest.orderStatus, opt => opt.MapFrom(src => src.Order != null
          ? src.Order.Status.ToString()
          : string.Empty))
          .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Order != null ? src.Order.OrderCost : 0))
          .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Order != null ? src.Order.Notes : string.Empty))
          .ReverseMap();
        #endregion

        #region Configuration of Shipping Type
        CreateMap<ShippingType, ShippingTypeDTO>()
       .ForMember(des => des.OrdersIds, opt => opt.MapFrom(src => src.Orders.Select(x => x.Id).ToList()))
       .ReverseMap();
        #endregion

        #region Configuration Of Order
        CreateMap<Order, OrderWithProductsDTO>().AfterMap((src, dest) =>
        {
            dest.Branch = src.Branch?.Name;
            dest.Region = src.Region?.Governorate;
            dest.City = src.CitySetting?.Name;
            dest.MerchantName = src.MerchantId;
            dest.Status = src.Status.ToString();
            dest.CustomerInfo = $"{src.CustomerName} {src.CustomerPhone}";
            dest.OrderCost = src.OrderCost + src.ShippingCost;
            dest.CourierId = src.CourierId;

        }).ReverseMap();

        CreateMap<Order, UpdateOrderDTO>().ReverseMap().ForMember(dest => dest.CitySettingId, opt => opt.MapFrom(src => src.City))
        .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch))
        .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.Region))
        .ForMember(dest => dest.ShippingTypeId, opt => opt.MapFrom(src => src.ShippingId))
        .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
        .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantName))
        .ForMember(dest => dest.Branch, opt => opt.Ignore())
        .ForMember(dest => dest.Region, opt => opt.Ignore())
        .ForMember(dest => dest.ShippingType, opt => opt.Ignore())
        .ForMember(dest => dest.CitySetting, opt => opt.Ignore());

        CreateMap<Order, AddOrderDTO>().ReverseMap().ForMember(dest => dest.CitySettingId, opt => opt.MapFrom(src => src.City))
         .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch))
         .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.Region))
         .ForMember(dest => dest.ShippingTypeId, opt => opt.MapFrom(src => src.ShippingId))
         .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType))
         .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchantName))
         .ForMember(dest => dest.Branch, opt => opt.Ignore())
         .ForMember(dest => dest.Region, opt => opt.Ignore())
         .ForMember(dest => dest.ShippingType, opt => opt.Ignore())
         .ForMember(dest => dest.CitySetting, opt => opt.Ignore());
        #endregion

        #region Configuration Of Branch

        CreateMap<Branch, BranchDTO>()
            .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.Region != null ? src.Region.Id : (int?)null))
            .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Governorate : null))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => u.FullName).ToList()))
            .ReverseMap();

        CreateMap<BranchToAddDTO, Branch>().ReverseMap();
        CreateMap<BranchToUpdateDTO, Branch>().ReverseMap();
        #endregion

        #region Configuration Of SpecialCityCost
        CreateMap<SpecialCityCost, SpecialCityCostDTO>()
           .ForMember(dest => dest.MerchantId, op => op.MapFrom(src => src.Merchant != null ? src.Merchant.Id : null))
           .ForMember(des => des.MerchantName, op => op.MapFrom(src => src.Merchant != null ? src.Merchant.FullName : null))
           .ForMember(des => des.CitySettingId, op => op.MapFrom(src => src.CitySetting != null ? src.CitySetting.Id : (int?)null))
           .ForMember(des => des.CitySettingName, op => op.MapFrom(src => src.CitySetting != null ? src.CitySetting.Name : null))
           .ReverseMap();
        #endregion

        #region Configuration Of CitySetting
        CreateMap<CitySetting, CitySettingDTO>()
         .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
         .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Governorate : null))
         .ForMember(dest => dest.UsersName, opt => opt.MapFrom(src => src.Users.Select(u => u.FullName).ToList()))
         .ForMember(dest => dest.OrderCost, opt => opt.MapFrom(src => src.Orders.Select(u => u.OrderCost).ToList()))
         .ForMember(dest => dest.UsersThatHasSpecialCityCost, opt => opt.MapFrom(src => src.SpecialPickups.Select(u => u.Merchant!.FullName).ToList()))
         .ReverseMap();
        CreateMap<CitySettingToAddDTO, CitySetting>().ReverseMap();
        CreateMap<CitySettingToUpdateDTO, CitySetting>().ReverseMap();
        #endregion

        #region Configuration Of SpecialCourierRegion
        CreateMap<SpecialCourierRegion, SpecialCourierRegionDTO>()
             .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
             .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Governorate : null))
             .ForMember(dest => dest.CourierId, opt => opt.MapFrom(src => src.Courier != null ? src.Courier.Id : null))
             .ForMember(dest => dest.CourierName, opt => opt.MapFrom(src => src.Courier != null ? src.Courier.FullName : null))
             .ReverseMap();
        #endregion
    }

}
