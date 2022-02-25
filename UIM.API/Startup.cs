using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sieve.Models;
using UIM.API.Helpers;
using UIM.API.Middlewares;
using UIM.Common;

namespace UIM.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UIM v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(env.IsDevelopment() ? "dev" : "prod");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandlingExt();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            ServiceExtensions.CreateRolesAndPwdUser(
                serviceProvider, EnvVars.DisableInitRolePwrUser).Wait();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextExt(Configuration.GetConnectionString("uimdb"));
            services.AddIdentityExt();
            services.AddAuthenticationExt();
            services.AddMapsterExt();
            services.AddDIContainerExt();

            // option patterns
            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));

            services.AddCorsExt();
            services.AddControllersExt();
            services.AddSwaggerExt();
        }
    }
}
