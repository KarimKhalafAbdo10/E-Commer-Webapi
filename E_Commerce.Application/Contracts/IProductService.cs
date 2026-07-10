using E_Commerce.Application.common;
using E_Commerce.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IProductService
    {
        Task<Result<ProductDto>> GetProductByIdAsync(int id ,CancellationToken ct=default);
        Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct=default);
        Task<Result<IReadOnlyList<TypesDto>>> GetAllTypesAsync(CancellationToken ct=default);
        Task<Result<IReadOnlyList<ProductDto>>> GetAllProductsAsync(ProductsQueryParams queryParams,CancellationToken ct=default);

    }
}
