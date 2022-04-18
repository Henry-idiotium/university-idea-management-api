using System.Net.Mime;

namespace UIM.Core.Helpers;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthenticationExt(this IServiceCollection services)
    {
        services.AddAuthentication(
            _ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        );
        return services;
    }

    public static IServiceCollection AddControllersExt(this IServiceCollection services)
    {
        services
            .AddControllers(
                options =>
                    options.ValueProviderFactories.Add(new SnakeCaseQueryValueProviderFactory())
            )
            .AddNewtonsoftJson(
                options =>
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
            )
            .ConfigureApiBehaviorOptions(
                opt =>
                    opt.InvalidModelStateResponseFactory = context =>
                    {
                        foreach (var item in context.ModelState)
                            foreach (var err in item.Value.Errors)
                                Console.WriteLine(err.ErrorMessage);

                        var result = new ValidationFailedResult();
                        result.ContentTypes.Add(MediaTypeNames.Application.Json);
                        return result;
                    }
            );
        return services;
    }

    public static IServiceCollection AddDIContainerExt(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<SieveProcessor>();
        services.AddScoped<ISieveProcessor, AppSieveProcessor>();
        // Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdeaService, IdeaService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<IGoogleDriveService, GoogleDriveService>();

        return services;
    }

    public static IServiceCollection AddDbContextExt(
        this IServiceCollection services,
        string localDbConnectionString
    )
    {
        if (EnvVars.CoreEnv == "development")
            services.AddDbContextPool<UimContext>(_ => _.UseSqlServer(localDbConnectionString));
        else
        {
            services.AddDbContextPool<UimContext>(
                _ =>
                    _.UseNpgsql(
                        $@"
						Port={EnvVars.Pgsql.Port};
						Server={EnvVars.Pgsql.Host};
						Database={EnvVars.Pgsql.Db};
						User Id={EnvVars.Pgsql.UserId};
						Pooling={EnvVars.Pgsql.Pooling}
						sslmode={EnvVars.Pgsql.SslMode};
						Password={EnvVars.Pgsql.Password};
						Trust Server Certificate={EnvVars.Pgsql.TrustServer};
						Integrated Security={EnvVars.Pgsql.IntegratedSecurity};"
                    )
            );
        }
        return services;
    }

    public static IServiceCollection AddIdentityExt(this IServiceCollection services)
    {
        if (EnvVars.CoreEnv == "development")
        {
            services
                .AddIdentity<AppUser, IdentityRole>(
                    _ =>
                    {
                        _.SignIn.RequireConfirmedEmail = false;
                        _.SignIn.RequireConfirmedAccount = false;
                        _.SignIn.RequireConfirmedPhoneNumber = false;
                    }
                )
                .AddEntityFrameworkStores<UimContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(
                    TokenOptions.DefaultProvider
                );

            services.Configure<IdentityOptions>(
                _ =>
                {
                    _.Password.RequiredLength = 0;
                    _.Password.RequireDigit = false;
                    _.Password.RequiredUniqueChars = 0;
                    _.Password.RequireUppercase = false;
                    _.Password.RequireLowercase = false;
                    _.Password.RequireNonAlphanumeric = false;
                }
            );
        }
        else
        {
            services
                .AddIdentity<AppUser, IdentityRole>(
                    _ =>
                    {
                        _.SignIn.RequireConfirmedEmail = false;
                        _.SignIn.RequireConfirmedAccount = true;
                        _.SignIn.RequireConfirmedPhoneNumber = false;
                    }
                )
                .AddEntityFrameworkStores<UimContext>()
                .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(
                    TokenOptions.DefaultProvider
                );

            services.Configure<IdentityOptions>(
                _ =>
                {
                    _.Password.RequireDigit = true;
                    _.Password.RequiredUniqueChars = 0;
                    _.Password.RequireUppercase = false;
                    _.Password.RequireLowercase = false;
                    _.Password.RequiredLength = default;
                }
            );
        }
        return services;
    }

    public static IServiceCollection AddMapperExt(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(UserProfile),
            typeof(IdeaProfile),
            typeof(RoleProfile),
            typeof(TagProfile),
            typeof(CommentProfile),
            typeof(DepartmentProfile),
            typeof(SubmissionProfile)
        );
        return services;
    }

    public static IServiceCollection AddSwaggerExt(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            _ =>
            {
                _.OperationFilter<SnakecasingParameOperationFilter>();
                _.DocumentFilter<LowercaseDocumentFilter>();
                _.SwaggerDoc("v1", new OpenApiInfo { Title = "UIM", Version = "v1" });
                _.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme."
                            + "\nEnter 'Bearer' [space] and then your token in the text input below."
                            + "\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    }
                );
                _.AddSecurityRequirement(
                    new OpenApiSecurityRequirement()
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
                    }
                );
            }
        );
        return services;
    }

    public static async Task InitRolesPowerUser(this IServiceCollection _, IServiceScope scope)
    {
        if (EnvVars.InitRolesPwrUser)
        {
            var userManager =
                (UserManager<AppUser>)scope.ServiceProvider.GetService(
                    typeof(UserManager<AppUser>)
                )!;
            var roleManager =
                (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(
                    typeof(RoleManager<IdentityRole>)
                )!;

            // Initializing custom roles
            var roleNames = new List<string>
            {
                EnvVars.Role.Staff,
                EnvVars.Role.PwrUser,
                EnvVars.Role.Manager,
                EnvVars.Role.Supervisor,
            };
            foreach (var name in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(name);
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole(name));
            }

            // Create a super user who will maintain the system
            var existingPwrUser = await userManager.FindByEmailAsync(EnvVars.PwrUserAuth.Email);
            if (existingPwrUser == null)
            {
                var pwrUser = new AppUser
                {
                    EmailConfirmed = true,
                    FullName = "Henry David",
                    Email = EnvVars.PwrUserAuth.Email,
                    UserName = EnvVars.PwrUserAuth.UserName,
                    CreatedDate = DateTime.Now,
                    IsDefaultPassword = false,
                    Avatar = await DiceBearHelpers.GetAvatarAsync(Sprites.Micah),
                };

                var createPowerUser = await userManager.CreateAsync(
                    pwrUser,
                    EnvVars.PwrUserAuth.Password
                );
                if (createPowerUser.Succeeded)
                    await userManager.AddToRoleAsync(pwrUser, EnvVars.Role.PwrUser);
            }
            else
            {
                // Add add poweruser to admin role if not
                var pwrUserRoles = await userManager.GetRolesAsync(existingPwrUser);
                if (pwrUserRoles.Count == 0)
                    await userManager.AddToRoleAsync(existingPwrUser, EnvVars.Role.PwrUser);
            }
        }
    }
}
