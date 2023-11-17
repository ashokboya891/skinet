using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entites;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification:BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductsSpecParams 
        productsSpecParams) :base(x=>
        (!productsSpecParams.BrandId.HasValue||x.ProductBrandId==productsSpecParams.BrandId) && 
        (!productsSpecParams.TypeId.HasValue || x.ProductTypeId==productsSpecParams.TypeId)
        )
        {

        }
    }
}