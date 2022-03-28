var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
{
    builder.Services
        .AddDbContextExt(Configuration.GetConnectionString("uimdb"))
        .AddMapperExt()
        .AddIdentityExt()
        .AddAuthenticationExt()
        .AddDIContainerExt()

        .Configure<SieveOptions>(Configuration.GetSection("Sieve"))

        .AddCorsExt()
        .AddControllersExt()
        .AddSwaggerExt();
}

var app = builder.Build();
var env = app.Environment;
var serviceProvider = app.Services;
// Configure the HTTP request pipeline.
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage()
            .UseSwagger()
            .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UIM v1"));
    }

    app.UseHttpsRedirection()
        .UseRouting()
        .UseCors("default")

        .UseAuthentication()
        .UseAuthorization()

        .UseHttpExceptionHandler()
        .UseJwt()

        .UseEndpoints(endpoints => endpoints.MapControllers());

    builder.Services.InitRolesPowerUser(serviceProvider.CreateScope()).Wait();
}
app.Run();