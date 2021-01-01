using System.IO;
using API.Extensions;
using API.Helpers;
using API.MIddleware;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // the StoreContext will be abailable only limited period given in a ServiceLifeTime.Scoped, which is a the http request entirety
            // once the request is finished, then the StoreContext is disposed
            services.AddDbContext<StoreContext>(x =>
            x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(x =>
            {
                x.UseSqlite(_config.GetConnectionString("IdentityConnection"));
            });

            ConfigureServices(services);
        }


        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<StoreContext>(x =>
            x.UseMySql(_config.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppIdentityDbContext>(x =>
            {
                x.UseMySql(_config.GetConnectionString("IdentityConnection"));
            });

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // This is refered to dependency injectin container. Any services that we want to add to our app, 
        // that we want to make available to other parts of app we add inside of this method.
        // When adding services the order does not matter, 
        // the only exception is when configuring controller configuration must be after adding controller 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllers();


            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions
                    .Parse(_config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddApplicationServices();

            services.AddIdentityServices(_config);

            services.AddSwaggerDocumentation();

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", plicy =>
                {
                    // it basically tells client app that 
                    // if it is running in an unsecured port we're not gonna return a header
                    // that's gonna allow our browser to display that information
                    plicy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This is where we add middleware. Our HTTP reques is going throw a pipeline and it it gonna hit our API server.
        // And then if we want to interact or do anything with that request as ut goes on its journey to enter the API server,
        // and then out of API server, then we've got an opportunity inside here to add middleware which can do various things with the request.
        // When adding a middleware in Configure method the ordering is important
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            // when we don't have an endpoint that matches the request it's gonna redirect to errors controller and pass in the statuc code
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            // if request comes to http (port:5000), then it is gonna redirect to https (port 5000 in our case)
            app.UseHttpsRedirection();

            // responsible to getting us to the controller
            app.UseRouting();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider =
                new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content/")),

                RequestPath = "/content"
            });

            app.UseCors("CorsPolicy");

            // authentication must be directly before the middleware use authorization
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            // When we start our app, it is gonna map all of our endpoints in a controller
            // so our API server where to send request on to
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
