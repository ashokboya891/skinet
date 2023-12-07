using Core.Entites;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public  ProductsWithTypesAndBrandsSpecification(ProductsSpecParams productsSpecParams)
        :base(x=>
        (string.IsNullOrEmpty(productsSpecParams.Search)|| x.Name.ToLower().Contains(productsSpecParams.Search))&& 
        (!productsSpecParams.BrandId.HasValue||x.ProductBrandId==productsSpecParams.BrandId) && 
        (!productsSpecParams.TypeId.HasValue || x.ProductTypeId==productsSpecParams.TypeId)
        )
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
            AddOrderBy(x=>x.Name);
            ApplyPaging(productsSpecParams.PageSize*(productsSpecParams.PageIndex-1),productsSpecParams.PageSize);
            if(!string.IsNullOrEmpty(productsSpecParams.Sort))
            {
                switch(productsSpecParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(d=>d.Price);
                        break;
                    default:
                       AddOrderBy(n=>n.Name);
                       break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id):base(x=>x.Id==id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }
    }
}