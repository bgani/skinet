using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }

        // fileters allow codes we run before or after specific stages in a request processing pipeline
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {       
            // get a reference for the cache service
            var cacheService = context.HttpContext.RequestServices
                  .GetRequiredService<IResponseCacheService>();

            // generate a key that will be idnetified in redis
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);;
           
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            // if we hove something in cache we send it backe to client, if we don't we move it to the controller
            if(!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                   
                  Content = cachedResponse,
                  ContentType = "application/json",
                  StatusCode = 200
                };

                context.Result = contentResult;
                return;
            }

            var executedContext = await next(); // move to controller
            if(executedContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService
                    .CacheResponseAsync(
                        cacheKey, 
                        okObjectResult.Value, 
                        TimeSpan.FromSeconds(_timeToLiveSeconds));
            }

        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");

            foreach(var (key, value) in request.Query.OrderBy(x => x.Key))
            {
              keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();
        }
    }
}