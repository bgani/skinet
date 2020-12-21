using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<AppUser>();

            // this is gonna allow our UserManager to work with our identity Database
            // btw identity doesn't have to be used with Entity Framework
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityDbContext>();

            builder.AddSignInManager<SignInManager<AppUser>>();

            // to have an access to SignInManager
            services.AddAuthentication();
            return services;
        }
        
    }
}