using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using napper_be.Helpers;
using napper_be.Middleware;
using napper_be.Repository;
using napper_be.Services;

namespace napper_be
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:8080").AllowAnyHeader().AllowAnyMethod();
                        builder.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod();
                    });
            });
            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<ISessionStorage>(storage => new FileSessionStorage(@"C:\Temp\Sessions\"));
            services.AddScoped<IUserStorage>(storage => new FileUserStorage(@"C:\Temp\Users\"));
            // services.AddScoped<ISessionStorage>(storage => new DBSessionStorage(Configuration));
            // services.AddScoped<IUserStorage>(storage => new DBUserStorage(Configuration));
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // Hook in the global error-handling middleware
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMiddleware<JwtMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
