using AutoMapper;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class ProductProfile :Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(des=>des.ProductBrand,opt=>opt.MapFrom(src=>src.ProductBrand.Name))
                .ForMember(des=>des,opt=>opt.MapFrom(src=>src.ProductType.Name));
            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypesDto>();
        }

    }
}
