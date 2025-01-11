using Microsoft.AspNetCore.Identity;
using MVCPractice.Models.Account;
using MVCPractice.Models.Activities;

namespace MVCPractice.Models
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<MVCPracticeDBContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!context.ActivityCategorys.Any())
            {
                context.ActivityCategorys.AddRange(
                    new ActivityCategory
                    {
                        Name = "單位1",
                        OrderIndex = 1,
                    },
                    new ActivityCategory
                    {
                        Name = "單位2",
                        OrderIndex = 2,
                    },
                    new ActivityCategory
                    {
                        Name = "單位3",
                        OrderIndex = 3,
                    }
                );
                context.SaveChanges();
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            var user = await userManager.FindByEmailAsync("admin@example.com");
            if (user == null)
            {
                user = new ApplicationUser { UserName = "admin", Email = "admin@example.com" };
                var result = await userManager.CreateAsync(user, "Aa123456!");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(user, ["Admin", "User"]);
                }
            }
        }
    }
}