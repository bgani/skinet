using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    // One of the things Apicontroller does is validation 
    // e.g It makes sure that the route parameter [HttpGet("{id}")] is actually an integer in case  
    [Route("api/[controller]")]
    public class BaseApiController: ControllerBase
    {
        
    }
}