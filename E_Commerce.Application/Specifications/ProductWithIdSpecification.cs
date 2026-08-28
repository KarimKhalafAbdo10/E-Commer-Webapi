using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithIdSpecification:BaseSpecification<Product,int>
    {
        public ProductWithIdSpecification(HashSet<int> productIds):base(p=>productIds.Contains(p.Id))
        {
            
        }
    }
}
