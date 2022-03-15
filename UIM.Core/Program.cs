var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;

// Add services to the container.
{
    var services = builder.Services;

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

var app = builder.Build();
var env = app.Environment;
var serviceProvider = app.Services;
// Configure the HTTP request pipeline.
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
    app.UseJwtExt();

    app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    ServiceExtensions.CreateRolesAndPwdUser(
        serviceProvider.CreateScope(), EnvVars.InitRolesPwrUser).Wait();
}
app.Run();