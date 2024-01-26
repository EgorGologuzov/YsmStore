using AutoMapper;
using YsmStore.API.Dto;
using YsmStore.API.Models;

namespace YsmStore.API.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerCreateDto, Customer>()
                .ReverseMap();
            CreateMap<CustomerReturnDto, Customer>()
                .ReverseMap();
            CreateMap<ProductInCartReturnDto, ProductInCart>()
                .ReverseMap();
            CreateMap<ProductReturnDto, Product>()
                .ReverseMap();
            CreateMap<OrderedProductDto, OrderedProduct>()
                .ReverseMap();
            CreateMap<ProductUpdateDto, Product>()
                .ReverseMap();
            CreateMap<OrderReturnDto, Order>()
                .ReverseMap();
            CreateMap<OrderCreateDto, Order>()
                .ReverseMap();
            CreateMap<OrderUpdateDto, Order>()
                .ReverseMap();
        }
    }
}
