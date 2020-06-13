using AutoMapper;
using BBIS_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
