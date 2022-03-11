namespace UIM.Core.Helpers;

public static class ServiceExtensions
{
    // Vanilla authorize attribute cannot obtain role claims
    public static void AddAuthenticationExt(this IServiceCollection services)
    {
        services
            .AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
    }

    public static void AddControllersExt(this IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
        });
    }

    public static void AddCorsExt(this IServiceCollection services)
    {
        services.AddCors(_ =>
        {
            _.AddPolicy("default", conf =>
                conf.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(EnvVars.ValidLocations));
        });
    }

    public static void AddDIContainerExt(this IServiceCollection services)
    {
        services.AddScoped<SieveProcessor>();
        services.AddScoped<ISieveProcessor, AppSieveProcessor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdeaService, IdeaService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
    }

    public static void AddDbContextExt(this IServiceCollection services, string localDbConnectionString)
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
    }

    public static void AddIdentityExt(this IServiceCollection services)
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
    }

    public static void AddMapperExt(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(UserProfile),
            typeof(IdeaProfile),
            typeof(CategoryProfile),
            typeof(DepartmentProfile),
            typeof(SubmissionProfile)
        );
    }

    public static void AddSwaggerExt(this IServiceCollection services)
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
    }

    public static async Task CreateRolesAndPwdUser(IServiceScope scope, bool disable)
    {
        if (!disable)
        {
            var userManager = (UserManager<AppUser>)scope.ServiceProvider.GetService(typeof(UserManager<AppUser>))!;
            var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))!;

            // Initializing custom roles
            var roleNames = new List<string>
                {
                    EnvVars.System.Role.Staff,
                    EnvVars.System.Role.PwrUser,
                    EnvVars.System.Role.Manager,
                    EnvVars.System.Role.Supervisor,
                };
            foreach (var name in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(name);
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole(name));
            }

            // Create a super user who will maintain the system
            var existingPwrUser = await userManager.FindByEmailAsync(EnvVars.System.PwrUserAuth.Email);
            if (existingPwrUser == null)
            {
                var pwrUser = new AppUser
                {
                    EmailConfirmed = true,
                    FullName = "Henry David",
                    Email = EnvVars.System.PwrUserAuth.Email,
                    UserName = EnvVars.System.PwrUserAuth.UserName,
                    CreatedDate = DateTime.UtcNow,
                };
                var createPowerUser = await userManager.CreateAsync(pwrUser,
                    EnvVars.System.PwrUserAuth.Password);

                if (createPowerUser.Succeeded)
                    await userManager.AddToRoleAsync(pwrUser, EnvVars.System.Role.PwrUser);
            }
        }
    }
}