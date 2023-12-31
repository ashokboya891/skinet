using Core.Entites;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Migrations
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext  context)
        {
            _context = context;
        }

        public StoreContext Context { get; }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
             return await _context.Products
             .Include(p=>p.ProductType)
             .Include(p=>p.ProductBrand)
             .FirstOrDefaultAsync(p=>p.Id==id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync( )
        {
           return await _context.Products.
           Include(p=>p.ProductType)
           .Include(p=>p.ProductBrand).
           ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
             return await _context.ProductTYpes.ToListAsync();

        }
    }
}