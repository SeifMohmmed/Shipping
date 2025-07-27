using AutoMapper;
using Shipping.Application.Abstraction.CourierReport.DTOs;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Application.Abstraction.ShippingType.DTOs;
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

    }

}
