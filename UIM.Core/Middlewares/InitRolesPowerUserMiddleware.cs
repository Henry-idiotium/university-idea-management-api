using Init = UIM.Core.Helpers.EnvVars.System;

namespace UIM.Core.Middlewares;

public static class InitRolesPowerUserMiddlewareExt
{
    public static IApplicationBuilder UseInitRolesPowerUser(this IApplicationBuilder app) =>
        app.UseMiddleware<InitRolesPowerUserMiddleware>();
}

public class InitRolesPowerUserMiddleware
{
    private readonly RequestDelegate _next;

    public InitRolesPowerUserMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (EnvVars.InitRolesPwrUser)
        {
            var userManager = (UserManager<AppUser>)context.RequestServices.GetService(typeof(UserManager<AppUser>))!;
            var roleManager = (RoleManager<IdentityRole>)context.RequestServices.GetService(typeof(RoleManager<IdentityRole>))!;

            // Initializing custom roles
            var roleNames = new List<string>
                {
                    Init.Role.Staff,
                    Init.Role.PwrUser,
                    Init.Role.Manager,
                    Init.Role.Supervisor,
                };
            foreach (var name in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(name);
                if (!roleExist)
                    await roleManager.CreateAsync(new IdentityRole(name));
            }

            #region init rando user
            var randoPwd = "qwe123";
            {
                var mgr = new AppUser
                {
                    EmailConfirmed = true,
                    FullName = "Spencer Yost",
                    Email = "manager@gmail.com",
                    UserName = "best_manager_ever_7861",
                    CreatedDate = DateTime.UtcNow,
                };
                var existingMgr = await userManager.FindByEmailAsync(mgr.Email);
                if (existingMgr == null)
                {
                    var createUser = await userManager.CreateAsync(mgr, randoPwd);
                    if (createUser.Succeeded) await userManager.AddToRoleAsync(mgr, Init.Role.Manager);
                }
            }
            {
                var supv = new AppUser
                {
                    EmailConfirmed = true,
                    FullName = "Jeff Wells",
                    Email = "bojejje@majpithu.st",
                    UserName = "aspernatur",
                    CreatedDate = DateTime.UtcNow,
                };
                var existingSupv = await userManager.FindByEmailAsync(supv.Email);
                if (existingSupv == null)
                {
                    var createUser = await userManager.CreateAsync(supv, randoPwd);
                    if (createUser.Succeeded) await userManager.AddToRoleAsync(supv, Init.Role.Supervisor);
                }
            }
            {
                var staff = new AppUser
                {
                    EmailConfirmed = true,
                    FullName = "Madge Valdez",
                    Email = "aptu@mitep.pt",
                    UserName = "unde",
                    CreatedDate = DateTime.UtcNow,
                };
                var existingStaff = await userManager.FindByEmailAsync(staff.Email);
                if (existingStaff == null)
                {
                    var createUser = await userManager.CreateAsync(staff, randoPwd);
                    if (createUser.Succeeded) await userManager.AddToRoleAsync(staff, Init.Role.Staff);
                }
            }
            #endregion

            // Create a super user who will maintain the system
            var existingPwrUser = await userManager.FindByEmailAsync(Init.PwrUserAuth.Email);
            if (existingPwrUser == null)
            {
                var pwrUser = new AppUser
                {
                    EmailConfirmed = true,
                    FullName = "Henry David",
                    Email = Init.PwrUserAuth.Email,
                    UserName = Init.PwrUserAuth.UserName,
                    CreatedDate = DateTime.UtcNow,
                };

                var createPowerUser = await userManager.CreateAsync(pwrUser, Init.PwrUserAuth.Password);
                if (createPowerUser.Succeeded)
                    await userManager.AddToRoleAsync(pwrUser, Init.Role.PwrUser);
            }
            else
            {
                // Add add poweruser to admin role if not 
                var pwrUserRoles = await userManager.GetRolesAsync(existingPwrUser);
                if (pwrUserRoles.Count == 0)
                    await userManager.AddToRoleAsync(existingPwrUser, Init.Role.PwrUser);
            }
        }
        await _next(context);
    }
}