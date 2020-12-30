using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        
        // Extending IServiceCollection
        // Adding Services
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
             services.AddScoped<ITokenService, TokenService>();
             services.AddScoped<IOrderService, OrderService>();
             services.AddScoped<IPaymentService, PaymentService>();
             services.AddScoped<IUnitOfWork, UnitOfWork>();

            // services.AddTransient got very short lifetime, the repo will be created and destroyed upon using individual method
            // sevices.AddSinglton got very long lifetime, the repo will be created when app starts and never be destroyed until the app shuts down
            // servces.AddScoped got the optimal lifetime, the instance of repo will be created when the http comes in, when the request is finished it disposes a controller and the repository
            services.AddScoped<IProductRepository, ProductRepository>();
            
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}