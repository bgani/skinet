using System.Collections.Generic;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    // One of the things Apicontroller does is validation 
    // e.g It makes sure that the route parameter [HttpGet("{id}")] is actually an integer in case  
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            
            var products = await _repo.GetProductsAsync();
            return Ok(products);
        }

        // As far as API cotroller concerned there is no difference between these two methods
        // What api uses to choose the correct endpoint we are going to is a comination of 
        // the Route e.g. [Route("api/[controller]")], the method e.g. [HttpGet("{id}")]
        // and any route parameters
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _repo.GetProductByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){
            // to get rid of 'Cannot implicitly convert type' error use Ok
            return  Ok(await _repo.GetProductBrandsAsync());
        }

         [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){
            return  Ok(await _repo.GetProductTypesAsync());
        }
    }
}