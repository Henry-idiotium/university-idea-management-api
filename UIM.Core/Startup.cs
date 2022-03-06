using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sieve.Models;
using UIM.Core.Helpers;
using UIM.Core.Middlewares;

namespace UIM.Core
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
            app.UseCors("default");

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
            services.AddMapperExt();
            services.AddIdentityExt();
            services.AddAuthenticationExt();
            services.AddDIContainerExt();

            services.Configure<SieveOptions>(Configuration.GetSection("Sieve"));

            services.AddCorsExt();
            services.AddControllersExt();
            services.AddSwaggerExt();
        }
    }
}
