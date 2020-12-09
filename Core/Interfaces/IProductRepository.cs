using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    // There are 2 ways to go with Repository Pattern
    // One option is to have a repository for every single type in your project
    // But for now we will stick with single repository 
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);

        // since we need to only return list, we can be more specific and use IReadOnlyList
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();


    }
}