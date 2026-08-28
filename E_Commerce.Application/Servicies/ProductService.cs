using AutoMapper;
using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Servicies
{
    internal class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork _unitOfWork,IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
             mapper=_mapper;
        }
        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync(ct);
            var mappedBrands = mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(mappedBrands);
        }

        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductsQueryParams queryParams, CancellationToken ct = default)
        {
            var specification = new ProductWithTypeAndBrandSpecification(queryParams);
               var products = await unitOfWork.GetRepository<Product,int>().GetAllAsync(specification,ct);
            var data = mapper.Map<IReadOnlyList<ProductDto>>(products);
            var countSpec = new ProductCountSpecification(queryParams);
            var countOfAllProducts = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpec,ct);
            var result = new PaginatedResult<ProductDto>(queryParams.PageIndex,queryParams.PageSize,countOfAllProducts,data);
               return Result<PaginatedResult<ProductDto>>.Ok(result);
        }


        public async Task<Result<IReadOnlyList<TypesDto>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            
            var    mappedTypes = mapper.Map<IReadOnlyList<TypesDto>>(await unitOfWork.GetRepository<ProductType,int>().GetAllAsync(ct));
            return Result<IReadOnlyList<TypesDto>>.Ok(mappedTypes);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {

            var spec=new ProductWithTypeAndBrandSpecification(id);
            var product= await unitOfWork.GetRepository<Product,int>().GetByIdAsync(spec, ct);
            if (product is null) return Error.NotFound("Product.NotFound",$"Product With Id{id} NOT Found");
            return mapper.Map<ProductDto>(product);

        }
    }
}
