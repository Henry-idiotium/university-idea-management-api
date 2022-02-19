using System;
using System.Net;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UIM.BAL.Services;
using UIM.BAL.Services.Interfaces;
using UIM.Common;
using UIM.Common.Helpers;
using UIM.Common.ResponseMessages;
using UIM.DAL.Data;
using UIM.DAL.Repositories;
using UIM.DAL.Repositories.Interfaces;
using UIM.Model.Dtos;
using UIM.Model.Entities;

namespace UIM.API.Helpers
{
    public static class ServiceExtensions
    {
        public static void AddAuthenticationExt(this IServiceCollection services)
        {
            services
                .AddAuthentication()
                .AddJwtBearer(_ =>
                {
                    _.RequireHttpsMetadata = true;
                    _.SaveToken = true;
                    _.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,

                        ClockSkew = TimeSpan.Zero,
                        ValidIssuers = EnvVars.Auth.ValidLocations.Split(';'),
                        ValidAudiences = EnvVars.Auth.ValidLocations.Split(';'),
                        IssuerSigningKey = new SymmetricSecurityKey(
                            EncryptHelpers.EncodeASCII(EnvVars.Jwt.Secret)),
                    };
                    _.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = (context) =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                throw new HttpException(HttpStatusCode.Unauthorized,
                                    ErrorResponseMessages.Unauthorized);

                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void AddCorsExt(this IServiceCollection services)
        {
            services.AddCors(_ =>
            {
                _.AddPolicy("DevelopmentPolicy", conf =>
                    conf.AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:3000",
                                    "https://localhost:7024",
                                    "http://localhost:5251"));
            });
        }

        public static void AddDIContainerExt(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IIdeaRepository, IdeaRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            // Services
            services.AddScoped<IIdeaService, IdeaService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtService, JwtService>();
            // others
            services.AddScoped<IMapper, ServiceMapper>();
        }

        public static void AddDbContextExt(this IServiceCollection services, string localDbConnectionString)
        {
            var isDev = EnvVars.AspNetCore.Environment.ToLower() == "development";
            if (isDev)
                services.AddDbContext<UimContext>(_ => _.UseSqlServer(localDbConnectionString));
            else
                services.AddDbContext<UimContext>(_ =>
                    _.UseNpgsql($@"
						Server={EnvVars.Pgsql.Host};
						Port={EnvVars.Pgsql.Port};
						User Id={EnvVars.Pgsql.UserId};
						Password={EnvVars.Pgsql.Password};
						Database={EnvVars.Pgsql.Db};
						sslmode={EnvVars.Pgsql.SslMode};
						Trust Server Certificate={EnvVars.Pgsql.TrustServer};
						Integrated Security={EnvVars.Pgsql.IntegratedSecurity};
						Pooling={EnvVars.Pgsql.Pooling}
					"));
        }

        public static void AddIdentityExt(this IServiceCollection services)
        {
            if (EnvVars.AspNetCore.Environment.ToLower() == "development")
            {
                services
                    .AddIdentity<AppUser, IdentityRole>(_ =>
                    {
                        _.SignIn.RequireConfirmedEmail = false;
                        _.SignIn.RequireConfirmedAccount = false;
                        _.SignIn.RequireConfirmedPhoneNumber = false;
                    })
                    .AddEntityFrameworkStores<UimContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

                services.Configure<IdentityOptions>(_ =>
                {
                    _.Password.RequireNonAlphanumeric = false;
                    _.Password.RequireUppercase = false;
                    _.Password.RequireLowercase = false;
                    _.Password.RequireDigit = false;
                    _.Password.RequiredLength = 0;
                    _.Password.RequiredUniqueChars = 0;
                });
            }
            else
            {
                services
                    .AddIdentity<AppUser, IdentityRole>(_ =>
                    {
                        _.SignIn.RequireConfirmedEmail = false;
                        _.SignIn.RequireConfirmedAccount = true;
                        _.SignIn.RequireConfirmedPhoneNumber = false;
                    })
                    .AddEntityFrameworkStores<UimContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);

                services.Configure<IdentityOptions>(_ =>
                {
                    _.Password.RequireUppercase = false;
                    _.Password.RequireLowercase = false;
                    _.Password.RequireDigit = true;
                    _.Password.RequiredLength = default;
                    _.Password.RequiredUniqueChars = 0;
                });
            }
        }

        public static void AddMapsterExt(this IServiceCollection services)
        {
            var config = new TypeAdapterConfig();

            config.NewConfig<AppUser, UserDetailsResponse>()
                .Map(dest => dest.Id, src => EncryptHelpers.EncodeBase64Url(src.Id))
                .Map(dest => dest.DateOfBirth, src => src.DateOfBirth.ToString("yy-MM-dd"));

            services.AddSingleton(config);
        }

        public static void AddSwaggerExt(this IServiceCollection services)
        {
            services.AddSwaggerGen(_ =>
                _.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "UIM",
                    Version = "v1"
                }));
        }
    }
}