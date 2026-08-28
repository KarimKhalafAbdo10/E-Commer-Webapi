using E_Commerce.Application.common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductCountSpecification : BaseSpecification<Product, int>
    {
        public ProductCountSpecification(ProductsQueryParams queryParams): base(p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value) && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value)
            && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || p.Name.ToLower().Contains(queryParams.SearchValue)
            ))
        {
            
        }
    }
}
