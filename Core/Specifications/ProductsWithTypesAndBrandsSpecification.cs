using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(string sort)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);

            if (!string.IsNullOrEmpty(sort)) 
            {
                switch(sort)
                {
                    case "priceAsc": 
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default: 
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
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