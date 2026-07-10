using E_Commerce.Application.common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithTypeAndBrandSpecification :BaseSpecification<Product,int>
    {

        public ProductWithTypeAndBrandSpecification(ProductsQueryParams queryParams)
            :base(p=>(!queryParams.BrandId.HasValue||p.BrandId== queryParams.BrandId.Value)&&(!queryParams.TypeId.HasValue||p.TypeId== queryParams.TypeId.Value)
            &&(string.IsNullOrWhiteSpace(queryParams.SearchValue)||p.Name.ToLower().Contains(queryParams.SearchValue)
            ))
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch (queryParams.Sort)
            {
                case ProductStringOptions.NameASC:
                    AddOrederBy(p => p.Name);
                    break;
                

                case ProductStringOptions.NameDESC: 
                    AddOrederByDescending(p => p.Name);
                    break;

                 case ProductStringOptions.PriceASC:
                    AddOrederBy(p => p.Price);
                    break;

                  case ProductStringOptions.PriceDESC:
                    AddOrederByDescending(p => p.Price);   
                    break;

                default:
                    AddOrederBy(p => p.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

        }
        public ProductWithTypeAndBrandSpecification(int id):base(x=>x.Id==id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }

    }
}
