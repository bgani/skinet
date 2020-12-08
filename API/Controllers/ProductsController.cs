using System.Collections.Generic;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using API.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]         
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            // how async works? ToListAsync parses of the request to delegate, that delegate queries the db
            // in the meantime the request of this thread is running on can go and handle other things
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // As far as API cotroller concerned there is no difference between these two methods
        // What api uses to choose the correct endpoint we are going to is a comination of 
        // the Route e.g. [Route("api/[controller]")], the method e.g. [HttpGet("{id}")]
        // and any route parameters
        [HttpGet("{id}")]
        public  async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}