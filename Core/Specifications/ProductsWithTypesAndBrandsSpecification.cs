using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }

        // We are replacing the generic expression from base constructor:  Expression<Func<T, bool>> 
        // with:  x=> x.Id == id
        public ProductsWithTypesAndBrandsSpecification(
            int id
        ) : base( x=> x.Id == id)
        {
            // adding inlcude statements
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}