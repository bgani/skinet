using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    // since we have route, but don't have an explicit [HttpGet] [HttpPost] ... it causes error in swagger
    // but we don't want it to be explicit, we want this to  handle any kinde of 
    // to make it ignored by swagger we set IgnoreApi = true
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController: BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}