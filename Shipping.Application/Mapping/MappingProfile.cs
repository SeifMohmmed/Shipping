using AutoMapper;
using Shipping.Application.Abstraction.Product.DTOs;
using Shipping.Domain.Entities;

namespace Shipping.Application.Mapping;
public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>().ReverseMap();

        CreateMap<Product, UpdateProductDTO>().ReverseMap();
    }

}
