using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.common
{
    public class ProductsQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SearchValue { get; set; }
        public  ProductStringOptions Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int DefaultPageSize= 5;
        private const int MaxPageSize= 10;
        private int pageSize = DefaultPageSize;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? DefaultPageSize : value);
        }

    }
}
