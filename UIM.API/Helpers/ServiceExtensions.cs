using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UIM.DAL.Data;
using UIM.Model.Entities;

namespace UIM.API.Helpers
{
    public static class ServiceExtensions
    {
        public static void AddDbContextExt(this IServiceCollection services, string localDbConnectionString)
        {
            var isDev = EnvHelpers.GetEnvVar("ASPNETCORE_ENVIRONMENT").ToLower() == "development";
            if (isDev)
                services.AddDbContext<UimContext>(option => option.UseSqlServer(localDbConnectionString));
            else
                services.AddDbContext<UimContext>(option =>
                    option.UseNpgsql($@"
						Server={EnvHelpers.GetEnvVar("PGSQL_HOST")};
						Port={EnvHelpers.GetEnvVar("PGSQL_PORT")};
						User Id={EnvHelpers.GetEnvVar("PGSQL_USER_ID")};
						Password={EnvHelpers.GetEnvVar("PGSQL_PASSWORD")};
						Database={EnvHelpers.GetEnvVar("PGSQL_DB")};
						sslmode={EnvHelpers.GetEnvVar("PGSQL_SSL_MODE")};
						Trust Server Certificate={EnvHelpers.GetEnvVar("PGSQL_TRUST_SERVER_CERTIFICATE")};
						Integrated Security={EnvHelpers.GetEnvVar("PGSQL_INTEGRATED_SECURITY")};
						Pooling={EnvHelpers.GetEnvVar("PGSQL_POOLING")}
					"));
        }

        public static void AddIdentityExt(this IServiceCollection services)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env.ToLowerInvariant() == "development")
            {
                services.AddIdentity<AppUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                })
                .AddEntityFrameworkStores<UimContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 0;
                    options.Password.RequiredUniqueChars = 0;
                });
            }
            else
            {
                services.AddIdentity<AppUser, IdentityRole>()
                        .AddEntityFrameworkStores<UimContext>()
                        .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

                services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = default;
                    options.Password.RequiredUniqueChars = 0;
                });
            }
        }

        public static void AddSwaggerExt(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "UIM",
                    Version = "v1"
                });
            });
        }
    }
}