using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCPractice.Models.Activities;

namespace MVCPractice.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVCPracticeDBContext(serviceProvider.GetRequiredService<DbContextOptions<MVCPracticeDBContext>>()))
            {
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        new IdentityRole
                        {
                            Name = "User",
                            NormalizedName = "USER"
                        },
                        new IdentityRole
                        {
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        }
                    );
                    context.SaveChanges();
                }

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
            }
        }
    }
}
