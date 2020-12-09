using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);

        // since we need to only return list, we can be more specific and use IReadOnlyList
        Task<IReadOnlyList<Product>> GetProductsAsync();
    }
}