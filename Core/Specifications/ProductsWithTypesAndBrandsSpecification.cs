using System;
using System.Linq.Expressions;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        // (!brandId.HasValue || x.ProductBrandId == brandId) - or else condition, 
        // if the !brandId.HasValue  is false then it executes what is on the right hand side of this condition
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
            : base(x =>
                 (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
                 (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) &&
                 (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            // 1st parameter is skip, 2nd is take
            // e.g if pageSize is 5, and if we want pageIndex 3.  So that means we skip 10 records and take next 5.
            ApplyPaging(
                productParams.PageSize * (productParams.PageIndex - 1),
                productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
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
        ) : base(x => x.Id == id)
        {
            // adding inlcude statements
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}