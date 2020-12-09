using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            // how async works? ToListAsync parses of the request to delegate, that delegate queries the db
            // in the meantime the request of this thread is running on can go and handle other things
            return await _context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }
    }
}