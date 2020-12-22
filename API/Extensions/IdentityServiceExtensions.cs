using System.Text;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {

        public static IServiceCollection AddIdentityServices(
                this IServiceCollection services,
                IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();

            // this is gonna allow our UserManager to work with our identity Database
            // btw identity doesn't have to be used with Entity Framework
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            builder.AddEntityFrameworkStores<AppIdentityDbContext>();

            builder.AddSignInManager<SignInManager<AppUser>>();

            // to have an access to SignInManager
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"], 
                        // we need to validate issuer
                        ValidateIssuer = true,
                        // by doing this we've overriden a default conf
                        ValidateAudience = false
                    };
                });
            return services;
        }

    }
}