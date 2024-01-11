
using API.DTOs;
using API.Errors;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Entites;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    // [ApiController]
    // [Route("api/[controller]")]
    public class ProductsController : BaseApiController
    {
       
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _productType;
        private readonly IGenericRepository<ProductBrand> _productBrand;
        private readonly IMapper _mapper;
      
        public ProductsController(IGenericRepository<Product> productRepo,
        IGenericRepository<ProductBrand> productBrand,IGenericRepository<ProductType> productType,IMapper mapper)
        {
            _mapper = mapper;
            _productBrand = productBrand;
            _productType = productType;
            _productRepo = productRepo; 
        }
        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductsSpecParams productsSpecParams)  //instead of writing like this (string sort,int? brandId,int? typeId ) we minimised out code by adding new class core.specifications.PorductsParams
        {
            var spec=new ProductsWithTypesAndBrandsSpecification(productsSpecParams);
            var countSpec=new ProductWithFiltersForCountSpecification(productsSpecParams);
            var totalItems=await _productRepo.CountAsync(countSpec);
            var products= await _productRepo.ListAsync(spec);
            var data=_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(products);
            return Ok(new Pagination<ProductToReturnDto>(productsSpecParams.PageIndex,productsSpecParams.PageSize,totalItems,data));
            // auto mapper removed below code
            // return products.Select(product=>new ProductToReturnDto
            // {
            //     Id=product.Id,
            //     Name=product.Name,
            //     Description=product.Description,
            //     PictureUrl=product.PictureUrl,
            //     Price=product.Price,
            //     ProductBrand=product.ProductBrand.Name,
            //     ProductType=product.ProductType.Name
                
            // }).ToList();
        }
        [Cached(600)]

        [HttpGet("{id}")]
        // below two lines represents that what this method will return to swagger
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]

        public  async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec=new ProductsWithTypesAndBrandsSpecification(id);
            var product=await _productRepo.GetEntityWithSpec(spec);
            if(product==null) return NotFound(new ApiResponse(404));
            return _mapper.Map<Product,ProductToReturnDto>(product);
            // auto mapper removed below code
            // return new ProductToReturnDto
            // {
            //     Id=product.Id,
            //     Name=product.Name,
            //     Description=product.Description,
            //     PictureUrl=product.PictureUrl,
            //     Price=product.Price,
            //     ProductBrand=product.ProductBrand.Name,
            //     ProductType=product.ProductType.Name

            // };
        }

        [Cached(600)]
        [HttpGet("brands")]
        // 
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrand.ListAllAsync());
        }

        
        [Cached(600)]
        [HttpGet("types")]
        // 
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypesBrands()
        {
            return Ok(await _productType.ListAllAsync());
        }
    }
}