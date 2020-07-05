using AutoMapper;
using BBIS_API.Models;

namespace BBIS_API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductItem, ProductGet>().ReverseMap();
            CreateMap<OrderItem, OrderGet>().ReverseMap();
            CreateMap<SellItem, SellGet>().ReverseMap();
        }
    }
}
