using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimplySecureApi.Data.DataContext;
using SimplySecureApi.Data.Models.Domain.Entity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SimplySecureApi.Data.Models.Authentication;

namespace SimplySecureApi.Data.Initialization
{
    public static class DataContextInitializer
    {
        public static Guid DefaultLocationGuid => new Guid("14b06356-1a28-486a-9c18-2a7b0543d276");

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

                    var userManger = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    foreach (var role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }

                    /*
                    var defaultLocation = await context.Locations.FirstOrDefaultAsync();

                    if (defaultLocation == null)
                    {
                        defaultLocation = new Location
                        {
                            Id = DataContextInitializer.DefaultLocationGuid,
                            Name = "Default Location",
                            Active = true,
                            IsSilentAlarm = false,
                            Armed = false,
                            Triggered = false
                        };

                        context.Locations.Add(defaultLocation);

                        await context.SaveChangesAsync();
                    }

                    */
                }
            }
        }
    }
}