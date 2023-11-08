
using Core.Entites;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
       
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductType> _productType;
        private readonly IGenericRepository<ProductBrand> _productBrand;
      
        public ProductsController(IGenericRepository<Product> productRepo,
        IGenericRepository<ProductBrand> productBrand,IGenericRepository<ProductType> productType)
        {
            _productBrand = productBrand;
            _productType = productType;
            _productRepo = productRepo; 
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var spec=new ProductsWithTypesAndBrandsSpecification();
            var products= await _productRepo.ListAsync(spec);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public  async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec=new ProductsWithTypesAndBrandsSpecification(id);
            return  await _productRepo.GetEntityWithSpec(spec);
        }

        [HttpGet("brands")]
        // 
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrand.ListAllAsync());
        }
        [HttpGet("types")]
        // 
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypesBrands()
        {
            return Ok(await _productType.ListAllAsync());
        }
    }
}