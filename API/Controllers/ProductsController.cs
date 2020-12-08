using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController: ControllerBase
    {
        
        [HttpGet]
        public string GetProducts(){
            return "this will be a list of productss";
        }

        // As far as API cotroller concerned there is no difference between these two methods
        // What api uses to choose the correct endpoint we are going to is a comination of 
        // the Route e.g. [Route("api/[controller]")], the method e.g. [HttpGet("{id}")]
        // and any route parameters
        [HttpGet("{id}")]
        public string GetProduct(int id){
            return "single product";
        }
    }
}