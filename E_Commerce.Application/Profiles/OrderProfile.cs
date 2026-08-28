using AutoMapper;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class OrderProfile :Profile
    {

        public OrderProfile()
        {
            CreateMap<OrderAddress, AddressDto>().ReverseMap();
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(o=>o.DeliveryMethod,o=>o.MapFrom(o=>o.DeliveryMethod.ShortName))
                .ForMember(o=>o.DeliveryMethodCost,o=>o.MapFrom(o=>o.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(o => o.ProductId, o => o.MapFrom(o => o.Product.ProductId))
                .ForMember(o => o.ProductName, o => o.MapFrom(o => o.Product.ProductName))
                .ForMember(o => o.PictureUrl, o => o.MapFrom<OrderItemPictureResolver>());


            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
