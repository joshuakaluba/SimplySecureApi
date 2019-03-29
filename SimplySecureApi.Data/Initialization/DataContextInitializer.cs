using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimplySecureApi.Data.DataContext;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimplySecureApi.Data.Initialization
{
    public static class DataContextInitializer
    {
        public static Claim DefaultUserClaim => new Claim("DefaultUserClaim", "DefaultUserAuthorization");

        public static string AdministratorRole => "Administrator";

        public static string UserRole => "User";

        private static readonly string[] Roles = new string[] { AdministratorRole, UserRole };

        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SimplySecureDataContext>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    await context.Database.MigrateAsync();

                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    foreach (var role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }
                }
            }
        }
    }
}