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
                Console.WriteLine("Creating fake users");
                string patientPassword = "Letmein01*";
                string doctorPassword = "Letmein01*";
                for (int i = 1; i <= 10; i++)
                {

                    string fakeDoctorName = $"Dr-{Faker.Name.First()}{i}";
                    Console.WriteLine($"Creating user {fakeDoctorName}");
                    var doctor = new IdentityUser { UserName = fakeDoctorName, Email = $"{fakeDoctorName}@healthapp.com", EmailConfirmed = true };

                    if (await userManager.FindByEmailAsync(fakeDoctorName) == null)
                    {
                        await userManager.CreateAsync(doctor, patientPassword);
                        await userManager.AddToRoleAsync(doctor, HealthAppRoles.Doctor);

                        Console.WriteLine($"{doctor.Id} is a doctor");  

                    }
                    for (int j = 1; j <= 10; j++)
                    {

                        string fakeUserName = $"P-{Faker.Name.First()}{j}";
                        Console.WriteLine($"Creating user {fakeUserName}");
                        var patient = new IdentityUser { UserName = fakeUserName, Email = $"{fakeUserName}@healthapp.com", EmailConfirmed = true };

                        if (await userManager.FindByEmailAsync(fakeUserName) == null)
                        {
                            await userManager.CreateAsync(patient, patientPassword);
                            await userManager.AddToRoleAsync(patient, HealthAppRoles.Patient);
                        }
                       
                    }


                }

            }
        }
    }
}
