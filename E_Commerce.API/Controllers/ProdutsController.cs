using E_Commerce.API.Attributes;
using E_Commerce.Application.common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace E_Commerce.API.Controllers
{
   
    public class ProdutsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProdutsController(IProductService productService)
        {
            _productService = productService;
        }

        //Get All Products
        [RedisCache(100)]
        [HttpGet]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]

        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery] ProductsQueryParams queryParams,CancellationToken ct)
        {
            var result =await _productService.GetAllProductsAsync(queryParams,ct);
            return ToActionResult(result);
        }

        //Get Product By Id

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id, CancellationToken ct)
        {
            var result = await _productService.GetProductByIdAsync(id, ct);
            return ToActionResult(result);
        }
        //Get All Brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct)
        {
            var result =await _productService.GetAllBrandsAsync(ct);
            return ToActionResult(result);

        }
        //Get All Types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypesDto>>> GetAllTypes(CancellationToken ct)
        {
            var result = await _productService.GetAllTypesAsync(ct);
            return ToActionResult(result);

        }



    }
}
