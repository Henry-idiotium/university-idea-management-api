using System.Net.Mime;

namespace UIM.Core.Helpers;

public static class ServiceExtensions
{
    // Vanilla authorize attribute cannot obtain role claims
    public static IServiceCollection AddAuthenticationExt(this IServiceCollection services)
    {
        services
            .AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
        return services;
    }

    public static IServiceCollection AddControllersExt(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
        })
        .ConfigureApiBehaviorOptions(opt =>
        {
            opt.InvalidModelStateResponseFactory = context =>
            {
                var result = new ValidationFailedResult();
                result.ContentTypes.Add(MediaTypeNames.Application.Json);
                return result;
            };
        });
        return services;
    }

    public static IServiceCollection AddCorsExt(this IServiceCollection services)
    {
        services.AddCors(_ =>
        {
            _.AddPolicy("default", conf =>
                conf.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(EnvVars.ValidOrigins));
        });
        return services;
    }

    public static IServiceCollection AddDIContainerExt(this IServiceCollection services)
    {
        services.AddScoped<SieveProcessor>();
        services.AddScoped<ISieveProcessor, AppSieveProcessor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdeaService, IdeaService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ISubmissionService, SubmissionService>();

        return services;
    }

    public static IServiceCollection AddDbContextExt(this IServiceCollection services, string localDbConnectionString)
    {
        if (EnvVars.CoreEnv == "development")
            services.AddDbContextPool<UimContext>(_ => _.UseSqlServer(localDbConnectionString));
        else
        {
            services.AddDbContextPool<UimContext>(_ =>
            {
                _.UseNpgsql($@"
                    Port={EnvVars.Pgsql.Port};
                    Server={EnvVars.Pgsql.Host};
                    Database={EnvVars.Pgsql.Db};
                    User Id={EnvVars.Pgsql.UserId};
                    Pooling={EnvVars.Pgsql.Pooling}
                    sslmode={EnvVars.Pgsql.SslMode};
                    Password={EnvVars.Pgsql.Password};
                    Trust Server Certificate={EnvVars.Pgsql.TrustServer};
                    Integrated Security={EnvVars.Pgsql.IntegratedSecurity};
                ");
            });
        }
        return services;
    }

    public static IServiceCollection AddIdentityExt(this IServiceCollection services)
    {
        if (EnvVars.CoreEnv == "development")
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
                _.Password.RequiredLength = 0;
                _.Password.RequireDigit = false;
                _.Password.RequiredUniqueChars = 0;
                _.Password.RequireUppercase = false;
                _.Password.RequireLowercase = false;
                _.Password.RequireNonAlphanumeric = false;
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
                _.Password.RequireDigit = true;
                _.Password.RequiredUniqueChars = 0;
                _.Password.RequireUppercase = false;
                _.Password.RequireLowercase = false;
                _.Password.RequiredLength = default;
            });
        }
        return services;
    }

    public static IServiceCollection AddMapperExt(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(UserProfile),
            typeof(IdeaProfile),
            typeof(SimpleEntitiesProfile),
            typeof(SubmissionProfile)
        );
        return services;
    }

    public static IServiceCollection AddSwaggerExt(this IServiceCollection services)
    {
        services.AddSwaggerGen(_ =>
        {
            _.DocumentFilter<LowercaseDocumentFilter>();
            _.SwaggerDoc("v1", new OpenApiInfo { Title = "UIM", Version = "v1" });
            _.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme." +
                              "\nEnter 'Bearer' [space] and then your token in the text input below." +
                              "\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            _.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
            });
        });
        return services;
    }
}