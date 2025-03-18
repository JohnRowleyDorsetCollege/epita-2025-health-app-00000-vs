using HealthApp.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthApp.Razor.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new string[] { HealthAppRoles.Patient, HealthAppRoles.Doctor, HealthAppRoles.Admin };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                string userName = "admin01@healtapp.com";
                if (await userManager.FindByEmailAsync(userName) == null)
                {
                    string password = "Admin@123";
                    var user = new IdentityUser { UserName = userName, Email = userName, EmailConfirmed = true };
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, HealthAppRoles.Admin);
                }
            }
        }
    }
}
