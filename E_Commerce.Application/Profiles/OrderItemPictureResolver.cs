using AutoMapper;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Domain.Entities.Order;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class OrderItemPictureResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly UrlSettings _Settings;

        public OrderItemPictureResolver(IOptions<UrlSettings> options)
        {
            _Settings = options.Value;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _Settings.BaseUrl.TrimEnd('/');
            var path = source.Product.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";


                }
    }
}
