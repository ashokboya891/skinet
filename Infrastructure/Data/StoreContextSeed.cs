
using System.Text.Json;
using Core.Entites;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
       public static async Task SeedAsync(StoreContext context)
       {
        if(!context.ProductBrands.Any())
        {
            var brandsData=File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
            var brands=JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            context.ProductBrands.AddRange(brands);
        }
         if(!context.ProductTYpes.Any())
        {
            var typesData=File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
            var types=JsonSerializer.Deserialize<List<ProductType>>(typesData);
            context.ProductTYpes.AddRange(types);
        }
         if(!context.Products.Any())
        {
            var ProductsData=File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
            var products=JsonSerializer.Deserialize<List<Product>>(ProductsData);
            context.Products.AddRange(products);
        }
        if(context.ChangeTracker.HasChanges())  await context.SaveChangesAsync();

       } 
        
    }
}