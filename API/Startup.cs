using API.Helpers;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // This is refered to dependency injectin container. Any services that we want to add to our app, 
        // that we want to make available to other parts of app we add inside of this method.
        // When adding services the order does not matter
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddTransient got very short lifetime, the repo will be created and destroyed upon using individual method
            // sevices.AddSinglton got very long lifetime, the repo will be created when app starts and never be destroyed until the app shuts down
            // servces.AddScoped got the optimal lifetime, the instance of repo will be created when the http comes in, when the request is finished it disposes a controller and the repository
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddControllers();
            // the StoreContext will be abailable only limited period given in a ServiceLifeTime.Scoped, which is a the http request entirety
            // once the request is finished, then the StoreContext is disposed
            services.AddDbContext<StoreContext>(x =>
            x.UseSqlite(_config.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // This is where we add middleware. Our HTTP reques is going throw a pipeline and it it gonna hit our API server.
        // And then if we want to interact or do anything with that request as ut goes on its journey to enter the API server,
        // and then out of API server, then we've got an opportunity inside here to add middleware which can do various things with the request.
        // When adding a middleware in Configure method the ordering is important
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if we are in a dev mode
            if (env.IsDevelopment())
            {
                // developer friendly exception page
                app.UseDeveloperExceptionPage();
            }

            // if request comes to http (port:5000), then it is gonna redirect to https (port 5000 in our case)
            app.UseHttpsRedirection();

            // responsible to getting us to the controller
            app.UseRouting();

            app.UseAuthorization();

            // When we start our app, it is gonna map all of our endpoints in a controller
            // so our API server where to send request on to
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
