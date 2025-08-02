﻿using AutoMapper;
using Shipping.Application.Abstraction.Branch.DTO;
using Shipping.Application.Abstraction.CitySettings.DTO;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Application.Abstraction.OrderReport.DTO;
using Shipping.Application.Abstraction.Orders.DTO;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Application.Abstraction.Region.DTO;
using Shipping.Application.Abstraction.ShippingType.DTOs;
using Shipping.Application.Abstraction.SpecialCityCost.DTO;
using Shipping.Application.Abstraction.SpecialCourierRegion.DTO;
using Shipping.Application.Abstraction.User.DTO;
using Shipping.Domain.Entities;
using Shipping.Domain.Enums;

namespace Shipping.Application.Mapping;
public class MappingProfile : Profile
{

    public MappingProfile()
    {
        #region Configuration of Product
        CreateMap<Product, ProductDTO>().ReverseMap();

        CreateMap<Product, UpdateProductDTO>().ReverseMap();
        #endregion

        #region Configration Of Region
        CreateMap<Region, RegionDTO>()
             .ForMember(dest => dest.CityName, op => op.MapFrom(src => src.CitySettings.Select(c => c.Name)))
             .ReverseMap();
        #endregion

        #region Configuration of CourierReport
        CreateMap<CourierReport, CourierReportDTO>()
          .ForMember(dest => dest.CourierName, opt => opt.MapFrom(src => src.Courier != null ? src.Courier.FullName : string.Empty))
          .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.Order.MerchantId))
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
        .ForMember(dest => dest.OrdersIds, opt => opt.MapFrom(src => src.Orders.Select(x => x.Id).ToList()))
       .ReverseMap();

        CreateMap<ShippingType, ShippingTypeUpdateDTO>()
        .ForMember(des => des.OrdersIds, opt => opt.MapFrom(src => src.Orders.Select(x => x.Id).ToList()))
        .ReverseMap();

        CreateMap<ShippingType, ShippingTypeAddDTO>()
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
            dest.CustomerAddress = $"{src.CustomerName} {src.CustomerPhone}";
            dest.OrderCost = src.OrderCost + src.ShippingCost;
            dest.CourierId = src.CourierId;

        }).ReverseMap();

        CreateMap<Order, UpdateOrderDTO>().ReverseMap().ForMember(dest => dest.CitySettingId, opt => opt.MapFrom(src => src.City))
        .ForMember(dest => dest.Id, opt => opt.Ignore()) // Explicitly ignore Id
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
         .ForMember(dest => dest.CitySetting, opt => opt.Ignore())
         .ForMember(dest => dest.CourierId, opt => opt.MapFrom(src => src.CourierId))
         .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
         .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.CustomerPhone))
         .ForMember(dest => dest.CustomerEmail, opt => opt.MapFrom(src => src.CustomerEmail))
         .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.CustomerAddress));



        #endregion

        #region  Configration Of OrderReport

        CreateMap<OrderReport, OrderReportDTO>()
        .ForMember(dest => dest.OrderId, op => op.MapFrom(src => src.OrderId))
        .ReverseMap();

        CreateMap<OrderReportDTO, OrderReport>()
        .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id since it's set from the route
        .ForMember(dest => dest.ReportDate, opt => opt.MapFrom(src => src.ReportDate))
        .ForMember(dest => dest.ReportDetails, opt => opt.MapFrom(src => src.ReportDetails))
        .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId));

        CreateMap<OrderReport, OrderReportToShowDTO>()
        .ForMember(dest => dest.Id, op => op.MapFrom(src => src.Id))
        .ForMember(dest => dest.ReportDate, op => op.MapFrom(src => src.ReportDate))
        .ForMember(dest => dest.IsDeleted, op => op.MapFrom(src => src.Order != null ? src.Order.IsDeleted : false))
        .ForMember(dest => dest.MerchantId, op => op.MapFrom(src => src.Order != null ? src.Order.MerchantId : string.Empty))
        .ForMember(dest => dest.CourierId, op => op.MapFrom(src => src.Order != null ? src.Order.CourierId : string.Empty))
        .ForMember(dest => dest.CustomerName, op => op.MapFrom(src => src.Order != null ? src.Order.CustomerName : string.Empty))
        .ForMember(dest => dest.CustomerPhone1, op => op.MapFrom(src => src.Order != null ? src.Order.CustomerPhone : string.Empty))
        .ForMember(dest => dest.RegionName, op => op.MapFrom(src => src.Order != null && src.Order.Region != null ? src.Order.Region.Governorate : string.Empty))
        .ForMember(dest => dest.CityName, op => op.MapFrom(src => src.Order != null && src.Order.CitySetting != null ? src.Order.CitySetting.Name : string.Empty))
        .ForMember(dest => dest.OrderCost, op => op.MapFrom(src => src.Order != null ? src.Order.OrderCost : 0))
        .ForMember(dest => dest.ShippingCost, op => op.MapFrom(src => src.Order != null ? src.Order.ShippingCost : 0))
        .ForMember(dest => dest.PaymentType, op => op.MapFrom(src => src.Order != null ? src.Order.PaymentType.ToString() : "No Payment"))
        .ForMember(dest => dest.OrderStatus,
    opt => opt.MapFrom(src => src.Order != null ? src.Order.Status : OrderStatus.Pending))
        .ReverseMap();
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
            .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.Merchant != null ? src.Merchant.Id : null))
            .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant != null ? src.Merchant.FullName : null))
            .ForMember(dest => dest.CitySettingId, opt => opt.MapFrom(src => src.CitySetting != null ? src.CitySetting.Id : (int?)null))
            .ForMember(dest => dest.CitySettingName, opt => opt.MapFrom(src => src.CitySetting != null ? src.CitySetting.Name : null))
            .ReverseMap();

        CreateMap<SpecialCityCost, SpecialCityAddDTO>()
            .ForMember(dest => dest.MerchantId, op => op.MapFrom(src => src.Merchant != null ? src.Merchant.Id : null))
            .ForMember(dest => dest.MerchantName, opt => opt.Ignore())
            .ForMember(dest => dest.CitySettingId, op => op.MapFrom(src => src.CitySetting != null ? src.CitySetting.Id : (int?)null))
            .ForMember(dest => dest.CitySettingName, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<SpecialCityUpdateDTO, SpecialCityCost>()
            .ForMember(dest => dest.Id, opt => opt.Ignore()) // 👈 prevent overwriting the key
            .ForMember(dest => dest.Merchant, opt => opt.Ignore())
            .ForMember(dest => dest.CitySetting, opt => opt.Ignore());


        #endregion

        #region Configuration Of CitySetting
        CreateMap<CitySetting, CitySettingDTO>()
         .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
         .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region != null ? src.Region.Governorate : null))
         .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.Select(u => u.FullName).ToList()))
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

        #region Configration Of Application User (Courier , Merchant , Employee)
        CreateMap<ApplicationUser, CourierDTO>()
             .ForMember(dest => dest.CourierId, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.CourierName, opt => opt.MapFrom(src => src.FullName))
             .ReverseMap();

        CreateMap<ApplicationUser, EmployeeDTO>()
             .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch!.Name))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
             .ReverseMap();


        CreateMap<AddEmployeeDTO, ApplicationUser>().AfterMap((src, dest) =>
        {
            dest.UserName = src.Email;
        });

        CreateMap<AddMerchantDTO, ApplicationUser>().AfterMap((src, dest) =>
        {
            dest.UserName = src.Email;
        });

        CreateMap<AddCourierDTO, ApplicationUser>().AfterMap((src, dest) =>
        {
            dest.UserName = src.Email;
        });

        CreateMap<SpecialCityCostDT0, SpecialCityCost>().ReverseMap();
        CreateMap<CourierRegionDT0, SpecialCourierRegion>().ReverseMap();
        CreateMap<SpecialCourierRegionDTO, SpecialCourierRegion>().ReverseMap();
        CreateMap<ApplicationUser, MerchantDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ReverseMap();

        CreateMap<ApplicationUser, AccountProfileDTO>();

        #endregion
    }

}
